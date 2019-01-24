using Art.ConfigurationReader.UI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art.ConfigurationReader.UI.Infrastructure.Repository
{
    public class ApplicationConfigRepository:IRepository<ApplicationConfig>
    {
        private IMongoDatabase mongoDatabase;

        public IMongoDatabase GetMongoDatabase()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            return mongoClient.GetDatabase("config");
        }

        public IEnumerable<ApplicationConfig> GetAll()
        {
            mongoDatabase = GetMongoDatabase();
            return mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").Find(FilterDefinition<ApplicationConfig>.Empty).ToList();
        }
        public void Create(ApplicationConfig appConfig)
        {
            mongoDatabase = GetMongoDatabase();
            mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").InsertOne(appConfig);
        }
        public ApplicationConfig Detail(int appConfigId)
        {
            mongoDatabase = GetMongoDatabase();
            return mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").Find<ApplicationConfig>(k => k.ID == appConfigId).FirstOrDefault();
        }
        public DeleteResult Delete(int appConfigId)
        {
            mongoDatabase = GetMongoDatabase();
            return mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").DeleteOne<ApplicationConfig>(k => k.ID == appConfigId);
        }
        public UpdateResult Update(ApplicationConfig appConfig)
        {
            mongoDatabase = GetMongoDatabase();
            var filter = Builders<ApplicationConfig>.Filter.Eq("ID", appConfig.ID);
            var updatestatement = Builders<ApplicationConfig>.Update.Set("ID", appConfig.ID);
            updatestatement = updatestatement.Set("Name", appConfig.Name);
            updatestatement = updatestatement.Set("Type", appConfig.Type);
            updatestatement = updatestatement.Set("Value", appConfig.Value);
            updatestatement = updatestatement.Set("IsActive", appConfig.IsActive);
            updatestatement = updatestatement.Set("ApplicationName", appConfig.ApplicationName);
            return mongoDatabase.GetCollection<ApplicationConfig>("ApplicationConfig").UpdateOne(filter, updatestatement);
        }
        
    }
}
