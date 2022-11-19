using CrackerProject.API.DataModels;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;
using Humanizer;
using MongoDB.Driver;

namespace CrackerProject.API.Repository
{
    public class SubSectionRepository : BaseRepository<SubSection>, ISubSectionRepository
    {
        public SubSectionRepository(IMongoContext context) : base(context)
        {
            base.DbSet = base.Context.GetCollection<SubSection>(nameof(Section).Pluralize());
        }

        public override async void Remove(Guid id)
        {
            var section = await base.DbSet.FindOneAndDeleteAsync(x => x.Id == id);
            var subsections_cursor = await base.DbSet.FindAsync(x => x.ParentBookSectionId == id);
            var subsections = subsections_cursor.ToList();

            if(subsections == null || subsections.Count() < 1)
                return;

            foreach(var subsection in subsections)
            {
                Remove(subsection.Id);
            }
        }
    }
}
