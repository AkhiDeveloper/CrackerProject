using CrackerProject.API.DataModels;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;
using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CrackerProject.API.Repository
{
    public class BookSectionRepository : BaseRepository<BookSection>, IBookSectionRepository
    {
        private readonly ISubSectionRepository _subsectionRepo;

        public BookSectionRepository(
            IMongoContext context, ISubSectionRepository subsectionRepo) 
            : base(context)
        {
            _subsectionRepo = subsectionRepo;
            base.DbSet = base.Context.GetCollection<BookSection>(nameof(Section).Pluralize());
        }

        public override async void Remove(Guid id)
        {
            var section = await base.DbSet.FindOneAndDeleteAsync(x => x.Id == id);
            var subsections = await _subsectionRepo.Find(x => x.ParentBookSectionId == id);
            foreach(var subsection in subsections)
            {
                _subsectionRepo.Remove(subsection.Id);
            }
        }
    }
}
