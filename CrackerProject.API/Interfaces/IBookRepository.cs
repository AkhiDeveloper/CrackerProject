using CrackerProject.API.Model;
using System.Linq.Expressions;

namespace CrackerProject.API.Interfaces
{
    public interface IBookRepository : IRepository<Model.Book, DataModels.Book, Guid>
    {
       
    }
    
}
