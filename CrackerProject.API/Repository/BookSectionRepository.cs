using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CrackerProject.API.Repository
{
    public class BookSectionRepository : BaseRepository<BookSection>, IBookSectionRepository
    {
        public BookSectionRepository(IMongoContext context) : base(context)
        {
            
        }

        public override async Task<IList<BookSection>> Find<U>(string fieldname, U fieldvalue)
        {
            return await base.Find(fieldname, fieldvalue);
        }
    }
}
