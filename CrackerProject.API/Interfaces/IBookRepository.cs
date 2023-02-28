using CrackerProject.API.Model.Book;
using System.Linq.Expressions;

namespace CrackerProject.API.Interfaces
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
       
    }
    
}
