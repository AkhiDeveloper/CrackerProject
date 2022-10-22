using CrackerProject.API.Models;

namespace CrackerProject.API.Services
{
    public interface IBookService
    {
        IEnumerable<Book> Get();
        Book Get(string id);
        Book Create(Book book);
        Book Update(string id, Book book);
        void Delete(string id);

    }
}
