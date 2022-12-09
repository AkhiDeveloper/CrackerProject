using AutoMapper;
using CrackerProject.API.Interfaces;
using CrackerProject.API.Model;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CrackerProject.API.Repository
{
    public class BookRepository : BaseRepository<Book, DataModels.Book, Guid>, IBookRepository
    {
        public BookRepository(IMongoContext context, IMapper mapper) 
            : base(context, mapper)
        {
            
        }

    }
}
