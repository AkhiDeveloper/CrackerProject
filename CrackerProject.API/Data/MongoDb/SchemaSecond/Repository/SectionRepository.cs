using CrackerProject.API.Interfaces;
using ServiceStack;
using AutoMapper;
using System.Linq.Expressions;
using MongoDB.Driver;
using Humanizer;
using MongoDB.Driver.Linq;
using DataModel = CrackerProject.API.Data.MongoDb.SchemaSecond.Model;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Repository
{
    public class SectionRepository : BaseRepository<Section, DataModel.Section, Guid>, ISectionRepository
    {
        private readonly IMongoCollection<DataModel.BookSection> _booksectionSet;
        private readonly IMongoCollection<DataModel.SubSection> _subsectionSet;

        public SectionRepository(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
            _booksectionSet = Context.GetCollection<DataModel.BookSection>
                (typeof(Model.Section).Name.Pluralize());
            _subsectionSet = Context.GetCollection<DataModel.SubSection>
                (typeof(Model.Section).Name.Pluralize());
        }

        public Task AddtoBook(Guid bookid, Section section)
        {
            var booksection = _mapper.Map<DataModel.BookSection>(section);
            booksection.BookId = bookid;
            Context.AddCommand(() => _booksectionSet.InsertOneAsync(booksection));
            return Task.CompletedTask;
        }

        public Task AddtoSection(Guid parentsectionid, Section section)
        {
            var subsection = _mapper.Map<DataModel.SubSection>(section);
            subsection.ParentSectionId = parentsectionid;
            Context.AddCommand(() => _subsectionSet.InsertOneAsync(subsection));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Section>> GetfromBook(Guid bookid)
        {
            var cursor = await _booksectionSet.FindAsync
                (x => x.BookId == bookid);
            var data = cursor.ToList();
            var objs = _mapper.Map<IEnumerable<Section>>(data);
            return objs;
        }

        public async Task<IEnumerable<Section>> GetfromBook(Guid bookid, Expression<Func<Section, bool>> predicate)
        {
            var dataexp = _mapper.Map<Expression<Func<DataModel.BookSection, bool>>>(predicate).Compile();
            Func<DataModel.BookSection, bool> idexp = x => x.BookId == bookid;
            var cursor = await _booksectionSet.FindAsync(x => dataexp(x) && idexp(x));
            var data = cursor.ToEnumerable();
            var objs = _mapper.Map<IEnumerable<Section>>(data);
            return objs;
        }

        public async Task<IEnumerable<Section>> GetfromSection(Guid parentsectionid)
        {
            var cursor = await _subsectionSet.FindAsync
                (x => x.ParentSectionId == parentsectionid);
            var data = cursor.ToList();
            var objs = _mapper.Map<IEnumerable<Section>>(data);
            return objs;
        }

        public async Task<IEnumerable<Section>> GetfromSection(Guid parentsectionid, Expression<Func<Section, bool>> predicate)
        {
            var dataexp = _mapper.Map<Expression<Func<DataModel.SubSection, bool>>>(predicate);
            var cursor = await _subsectionSet.FindAsync(dataexp);
            var data = cursor.ToList();
            var objs = _mapper.Map<IEnumerable<Section>>(data);
            return objs;
        }

        public async Task<bool> IsExistInBook(Guid bookid, Guid sectionid)
        {
            var cursor = await _booksectionSet.FindAsync
                (x => x.BookId == bookid);
            return cursor.ToList().Any();
        }

        public async Task<bool> IsExistInSection(Guid parentsectionid, Guid sectionid)
        {
            var cursor = await _subsectionSet.FindAsync
                (x => x.ParentSectionId == parentsectionid);
            return cursor.ToList().Any();
        }

        public async Task MovetoBook(Guid sectionid, Guid destination_book_id)
        {
            var sectioncursor = await _booksectionSet.FindAsync(x => x.Id == sectionid);
            var section = await sectioncursor.FirstOrDefaultAsync();
            if (section == null)
            {
                throw new NullReferenceException
                    ($"Section with id = {sectionid} is not found.");
            }

            section.BookId = destination_book_id;
            Context.AddCommand(() =>
            _booksectionSet.ReplaceOneAsync(x => x.Id == sectionid, section));
        }

        public async Task MovetoSection(Guid sectionid, Guid targetsectionid)
        {
            var sectioncursor = await _subsectionSet.FindAsync(x => x.Id == sectionid);
            var section = await sectioncursor.FirstOrDefaultAsync();
            if (section == null)
            {
                throw new NullReferenceException
                    ($"Section with id = {sectionid} is not found.");
            }
            var parentsectioncursor = await DbSet.FindAsync(x => x.Id == targetsectionid);
            var parentsection = await parentsectioncursor.FirstOrDefaultAsync();
            if (parentsection == null)
            {
                throw new NullReferenceException
                    ($"Book with id = {targetsectionid} is not found.");
            }
            section.ParentSectionId = parentsection.Id;
            Context.AddCommand(() =>
            _subsectionSet.ReplaceOneAsync(x => x.Id == sectionid, section));
        }

        public async Task RemoveBookSections(Guid bookid, DeleteType deleteType)
        {
            if (bookid == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            Context.AddCommand(() => _booksectionSet
                .DeleteManyAsync(x => x.BookId == bookid));
            if (deleteType == DeleteType.ElementOnly)
            {
                return;
            }
            if (deleteType == DeleteType.AssociatedAlso)
            {
                var booksections = await GetfromBook(bookid);
                foreach (var booksection in booksections)
                {
                    await RemoveSubSections
                        (booksection.Id, DeleteType.AssociatedAlso);
                }
                return;
            }
            return;
        }

        public async Task RemoveSubSections(Guid parentsectionid, DeleteType deleteType)
        {
            if (parentsectionid == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            Context.AddCommand(() => _subsectionSet
                .DeleteManyAsync(x => x.ParentSectionId == parentsectionid));
            if (deleteType == DeleteType.ElementOnly)
            {
                return;
            }
            if (deleteType == DeleteType.AssociatedAlso)
            {
                var booksections = await GetfromSection(parentsectionid);
                foreach (var booksection in booksections)
                {
                    await RemoveSubSections
                        (booksection.Id, DeleteType.AssociatedAlso);
                }
                return;
            }
            return;
        }
    }
}
