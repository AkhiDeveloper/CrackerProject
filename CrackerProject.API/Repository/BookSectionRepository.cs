using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;

namespace CrackerProject.API.Repository
{
    public class BookSectionRepository : BaseRepository<BookSection>, IBookSectionRepository
    {
        public BookSectionRepository(IMongoContext context) : base(context)
        {
        }
    }
}
