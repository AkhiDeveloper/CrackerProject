using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using CrackerProject.API.Interfaces;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Humanizer;
using System.Linq.Expressions;
using AutoMapper;

namespace CrackerProject.API.Repository
{
    public abstract class BaseRepository<TModel, TDataModel, TIdentifier> 
        : IRepository<TModel, TDataModel, TIdentifier> where TModel : class where TDataModel : class
    {
        protected readonly IMapper _mapper;
        protected readonly IMongoContext Context;
        protected IMongoCollection<TDataModel> DbSet;

        protected BaseRepository(IMongoContext context, IMapper mapper)
        {
            Context = context;
            _mapper = mapper;
            DbSet = Context.GetCollection<TDataModel>(typeof(TDataModel).Name.Pluralize());
        }

        public virtual void AddAsync(TModel obj)
        {
            var data = _mapper.Map<TDataModel>(obj);
            Context.AddCommand(() => DbSet.InsertOneAsync(data));
        }

        public virtual async Task<TModel> GetById(TIdentifier id)
        {
            var cursor = await DbSet.FindAsync(Builders<TDataModel>.Filter.Eq("_id", id));
            var data = await cursor.SingleOrDefaultAsync();
            var obj = _mapper.Map<TModel>(data);
            return obj;
        }

        public virtual async Task<IEnumerable<TModel>> GetAll()
        {
            var cursor = await DbSet.FindAsync(Builders<TDataModel>.Filter.Empty);
            var data = cursor.ToList();
            var obj = _mapper.Map<IEnumerable<TModel>>(data);
            return obj;
        }

        public virtual void UpdateAsync(TModel obj)
        {
            var data = _mapper.Map<TDataModel>(obj);
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TDataModel>.Filter.Eq("_id", obj.GetId()), data));
        }

        public virtual void RemoveAsync(TIdentifier id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TDataModel>.Filter.Eq("_id", id)));
        }

        public virtual async Task<IList<TModel>> Find<U>(string fieldname, U fieldvalue)
        {
            var result = await DbSet.FindAsync(Builders<TDataModel>.Filter.Eq(fieldname,fieldvalue));
            return _mapper.Map<IList<TModel>>(result.ToList());
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public virtual async Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> expression)
        {
            var dataexp = _mapper.Map<Expression<Func<TDataModel, bool>>>(expression);
            var cursor = await DbSet.FindAsync(dataexp);
            var data = await cursor.ToListAsync();
            var objs = _mapper.Map<IEnumerable<TModel>>(data);
            return objs;
        }

        public async Task<bool> IsExist(TIdentifier id)
        {
            var cursor = await DbSet.FindAsync<TDataModel>(Builders<TDataModel>.Filter.Eq("_id", id));
            var result = await cursor.ToListAsync();
            if(result.Count < 1) return false;
            return true;
        }
    }
}
