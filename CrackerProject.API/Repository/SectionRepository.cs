using CrackerProject.API.DataModels;
using CrackerProject.API.Interfaces;
using ServiceStack;

namespace CrackerProject.API.Repository
{
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(IMongoContext context) : base(context)
        {
        }
    }
}
