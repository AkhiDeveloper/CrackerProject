using BookManager.API.Models;
using System.Linq.Expressions;

namespace BookManager.API.ServiceProvider
{
    public interface IBookManager
    {
        public Task CreateBook(Book book);

        public Task DeleteBook(Guid book_id);

        public Task<Book> GetBook(Guid book_id);

        public Task<IEnumerable<Book>> GetAllBooks();

        public Task<IEnumerable<Book>> GetAllBooks(Expression<Func<Book, bool>> predicate);

        public Task ChangeBookDescription(Guid book_id, string new_description);

        public Task ChangeBookImage(Guid book_id, Stream image_stream, string extension);

        public Task<bool> IsBookExist(Guid book_id);

        public Task<bool> HasImage(Guid book_id);

        public Task SaveImage(Guid book_id, Stream image_stream, string extension);

        public Task DeleteImage(Guid book_id);
    }
}
