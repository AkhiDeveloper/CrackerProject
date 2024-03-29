﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrackerProject.API.Data.Interfaces
{
    public interface IRepository<TModel, Identifier> : IDisposable where TModel : class
    {
        Task<bool> IsExist(Identifier id);
        void AddAsync(TModel obj);
        Task<TModel> GetById(Identifier id);
        Task<IEnumerable<TModel>> GetAll();
        void UpdateAsync(TModel obj);
        void RemoveAsync(Identifier id);
        Task<IList<TModel>> Find<U>(string fieldname, U fieldvalue);
        Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> expression);
    }
}