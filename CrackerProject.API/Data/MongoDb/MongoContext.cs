using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using CrackerProject.API.Interfaces;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrackerProject.API.Settings;

namespace CrackerProject.API.Data.MongoDb
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands;
        private readonly MongoDbSettings _dbsettings;

        public MongoContext(MongoDbSettings dbsettings)
        {
            _dbsettings = dbsettings;

            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();
        }

        public async Task<int> SaveChanges()
        {
            ConfigureMongo();

            using (Session = await MongoClient.StartSessionAsync())
            {
                try
                {
                    Session.StartTransaction();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);
                try
                {
                    await Session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }

            }

            return _commands.Count;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null)
            {
                return;
            }

            // Configure mongo (You can inject the config, just to simplify)
            MongoClient = new MongoClient(_dbsettings.ConnectionString);

            Database = MongoClient.GetDatabase(_dbsettings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();

            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }
    }
}