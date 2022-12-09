using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrackerProject.API.Interfaces
{
    public interface IRepository<TModel, TDataModel, Identifier> : IDisposable where TModel : class where TDataModel:class
    {
        Task<bool> IsExist(Identifier id);
        void Add(TModel obj);
        Task<TModel> GetById(Identifier id);
        Task<IEnumerable<TModel>> GetAll();
        void Update(TModel obj);
        void RemoveAsync(Identifier id);
        Task<IList<TModel>> Find<U>(string fieldname, U fieldvalue);
        Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> expression);
    }
}