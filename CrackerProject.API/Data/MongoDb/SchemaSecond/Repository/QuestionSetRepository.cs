using AutoMapper;
using DataModel = CrackerProject.API.Data.MongoDb.SchemaSecond.Model;
using CrackerProject.API.Interfaces;
using Humanizer;
using MongoDB.Driver;
using ServiceStack;
using System.Linq.Expressions;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.Data.MongoDb.SchemaSecond.Repository
{
    public class QuestionSetRepository : BaseRepository<QuestionSet, DataModel.QuestionSet, Guid>, IQuestionSetRepository
    {
        private readonly IMongoCollection<Model.Section> _sections;
        public QuestionSetRepository(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
            _sections = Context.GetCollection<Model.Section>
                (nameof(Model.Section).Pluralize());
        }

        private async Task<Model.Section> GetParentSection(Guid questionset_id)
        {
            var originsectioncursor = await _sections.FindAsync(x =>
            x.QuestionSets.Any(q =>
            q.Id == questionset_id));
            var originsection = originsectioncursor.FirstOrDefault();
            if (originsection == null)
            {
                throw new NullReferenceException
                    ($"Questionset with id ={questionset_id} is not found.");
            }
            return originsection;
        }

        public override void AddAsync(QuestionSet obj)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAsync(QuestionSet obj)
        {
            var originsection = GetParentSection(obj.Id).GetResult();
            var questionset = originsection.QuestionSets
                .FirstOrDefault(x => x.Id == obj.Id);
            _mapper.Map(obj, questionset);
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == originsection.Id, originsection));
        }

        public override Task<IEnumerable<QuestionSet>> Find(Expression<Func<QuestionSet, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<QuestionSet>> Find<U>(string fieldname, U fieldvalue)
        {
            throw new NotImplementedException();
        }

        public override async Task<QuestionSet> GetById(Guid id)
        {
            var parentsectiondata = await GetParentSection(id);
            var questionsetdata = parentsectiondata.QuestionSets.FirstOrDefault(x => x.Id == id);
            var questionset = _mapper.Map<QuestionSet>(questionsetdata);
            return questionset;
        }

        public override Task<IEnumerable<QuestionSet>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async void RemoveAsync(Guid id)
        {
            var parentsectiondata = await GetParentSection(id);
            var questionsetdata = parentsectiondata.QuestionSets.FirstOrDefault(x => x.Id == id);
            parentsectiondata.QuestionSets.Remove(questionsetdata);
            Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == id, parentsectiondata));
        }

        public async Task AddtoSection(Guid sectionid, QuestionSet questionset)
        {
            var sectioncursor = await _sections.FindAsync(x => x.Id == sectionid);
            var section = sectioncursor.FirstOrDefault();
            if (section == null)
            {
                throw new NullReferenceException
                    ("Section with id ={id} is not found.");
            }
            var questionsetdata = _mapper.Map<Model.QuestionSet>(questionset);
            if (section.QuestionSets == null)
            {
                section.QuestionSets = new List<Model.QuestionSet>();
            }
            section.QuestionSets.Add(questionsetdata);
            Context.AddCommand(() => _sections.ReplaceOneAsync(x => x.Id == sectionid, section));
        }

        public async Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid)
        {
            var sectioncursor = await _sections.FindAsync(x => x.Id == sectionid);
            var section = sectioncursor.FirstOrDefault();
            if (section == null)
            {
                throw new NullReferenceException
                    ("Section with id ={id} is not found.");
            }
            var questionsetmodel = _mapper.Map<IEnumerable<QuestionSet>>(section.QuestionSets);
            return questionsetmodel;
        }

        public Task<IEnumerable<QuestionSet>> GetfromSection(Guid sectionid, Expression<Func<Section, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistInSection(Guid sectionid, Guid questionset_id)
        {
            var sectioncursor = await _sections
                .FindAsync(x => x.Id == sectionid);
            var section = sectioncursor.FirstOrDefault();
            if (section == null)
            {
                throw new NullReferenceException
                    ($"Section with id ={sectionid} is not found.");
            }
            var isexist = section.QuestionSets.Any(x => x.Id == questionset_id);
            return isexist;
        }

        public async Task MovetoSection(Guid questionset_id, Guid section_id)
        {
            var targetsectioncursor = await _sections.FindAsync(x => x.Id == section_id);
            var targetsection = targetsectioncursor.FirstOrDefault();
            if (targetsection == null)
            {
                throw new NullReferenceException
                    ($"Target Section with id ={section_id} is not found.");
            }
            if (targetsection.QuestionSets.Any(x => x.Id == questionset_id))
            {
                return;
            }

            var originsection = await GetParentSection(questionset_id);

            var questionsetdata = originsection.QuestionSets
                .FirstOrDefault(x => x.Id == questionset_id);
            if (!originsection.QuestionSets.Remove(questionsetdata))
            {
                throw new NullReferenceException
                    ($"Questionset with id ={questionset_id} is not found.");
            }
            targetsection.QuestionSets.Add(questionsetdata);
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == targetsection.Id, targetsection));
            Context.AddCommand(() => _sections
            .ReplaceOneAsync(x => x.Id == originsection.Id, targetsection));
        }

        public async Task RemoveSectionQuestionSets(Guid sectionid, DeleteType deleteType)
        {
            var originsectioncursor = await _sections
                .FindAsync(x => x.Id == sectionid);
            var originsection = originsectioncursor.FirstOrDefault();
            if (originsection == null)
            {
                throw new NullReferenceException
                    ($"Target Section with id ={sectionid} is not found.");
            }
            originsection.QuestionSets.Clear();
            Context.AddCommand(() => _sections.ReplaceOneAsync(
                x => x.Id == sectionid, originsection));
        }
    }
}
