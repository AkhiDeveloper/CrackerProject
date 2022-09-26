using CrackerProject.API.Models;
using CrackerProject.API.Settings;
using Humanizer;
using MongoDB.Driver;

namespace CrackerProject.API.Services
{
    public class BookService : IBookService
    {
        private readonly IMongoCollection<Book> _books;
        public BookService(
            MongoDbSettings settings,
            IMongoClient client)
        {
            var db = client.GetDatabase(settings.DatabaseName);

            var collectionname = nameof(Book).Pluralize();

            _books = db
                .GetCollection<Book>(
                settings.CollectionNames
                .First(x => x.Equals(collectionname)
                ));

            if(_books == null)
            {
                db.CreateCollection(nameof(Book).Pluralize());

                _books = db
                .GetCollection<Book>(
                settings.CollectionNames
                .First(x => x.Equals(collectionname)
                ));
            }

        }
        public Book Create(Book? book)
        {
            if(book == null)
            {
                throw new ArgumentNullException();
            }

            _books.InsertOne(book);

            var savedbook = _books.Find(book.Id);

            if(savedbook == null)
            {
                throw new Exception($"Failed to save the {nameof(book)} in collecton {nameof(book).Pluralize()} of id = {book.Id}");
            }

            return book;
            
        }

        public void Delete(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }

            _books.DeleteOne(id);
            
        }

        public IEnumerable<Book> Get()
        {
            return _books.Find(book => true).ToList();
            
        }

        public Book Get(string id)
        {
            throw new NotImplementedException();
        }

        public Book Update(string id, Book book)
        {
            throw new NotImplementedException();
        }
    }
}
