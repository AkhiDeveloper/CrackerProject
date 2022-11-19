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

namespace CrackerProject.API.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            Context = context;

            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name.Pluralize());
        }

        public virtual void Add(TEntity obj)
        {
            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        public virtual void Remove(Guid id)
        {
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public virtual async Task<IList<TEntity>> Find<U>(string fieldname, U fieldvalue)
        {
            var result = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq(fieldname,fieldvalue));
            return await result.ToListAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            var result = await DbSet.FindAsync(expression);
            return await result.ToListAsync();
        }

        public async Task<bool> IsExist(Guid id)
        {
            var cursor = await DbSet.FindAsync<TEntity>(Builders<TEntity>.Filter.Eq("_id", id));
            var result = await cursor.ToListAsync();
            if(result.Count < 1) return false;
            return true;
        }
    }
}
