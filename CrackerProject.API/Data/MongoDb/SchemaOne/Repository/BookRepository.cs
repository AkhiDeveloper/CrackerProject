using AutoMapper;
using DataModel = CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using CrackerProject.API.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;
using CrackerProject.API.Model.Book;

namespace CrackerProject.API.Data.MongoDb.SchemaOne.Repository
{
    public class BookRepository : BaseRepository<Book, DataModel.Book, Guid>, IBookRepository
    {
        public BookRepository(IMongoContext context, IMapper mapper)
            : base(context, mapper)
        {

        }

    }
}
