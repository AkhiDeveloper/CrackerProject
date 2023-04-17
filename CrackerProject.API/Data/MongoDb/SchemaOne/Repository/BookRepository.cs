using AutoMapper;
using DataModel = CrackerProject.API.Data.MongoDb.SchemaOne.Model;
using MongoDB.Driver;
using System.Linq.Expressions;
using CrackerProject.API.Model.Book;
using CrackerProject.API.Data.Interfaces;

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
