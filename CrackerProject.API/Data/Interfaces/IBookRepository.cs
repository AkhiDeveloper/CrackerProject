using CrackerProject.API.Model.Book;
using System.Linq.Expressions;

namespace CrackerProject.API.Data.Interfaces
{
    public interface IBookRepository : IRepository<Book, Guid>
    {

    }

}
