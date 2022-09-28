using CrackerProject.API.Interfaces;
using CrackerProject.API.Models;

namespace CrackerProject.API.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(IMongoContext context) : base(context)
        {
        }
    }
}
