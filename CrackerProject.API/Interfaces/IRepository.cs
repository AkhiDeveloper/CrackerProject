using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrackerProject.API.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<bool> IsExist(Guid id);
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Remove(Guid id);
        Task<IList<TEntity>> Find<U>(string fieldname, U fieldvalue);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression);
    }
}