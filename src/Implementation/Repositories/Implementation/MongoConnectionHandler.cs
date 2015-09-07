using System;
using Implementation.Entities;
using Implementation.Entities.Interfaces;
using Implementation.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Implementation.Repositories.Implementation
{
    /// <summary>
    /// Connects to mongoservice and sets mongo database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoConnectionHandler<T> where T : IMongoEntity
    {
        public IMongoCollection<T> MongoCollection { get; private set; }

        public MongoConnectionHandler(MongoConfigService mongoConfigService)
        {
            if(mongoConfigService == null)
                throw new ArgumentNullException("mongoConfigService", "mongoConfigService can't be null");

            string url = mongoConfigService.GetConnectionUrl();
            var mongoDbName = mongoConfigService.GetSourceDatabaseName();

            var client = new MongoClient(url);
            IMongoDatabase database = client.GetDatabase(mongoDbName);
            if (database == null)
            {
                throw new DatabaseNotFoundException(mongoDbName);
            }

            //// Get a reference to the collection object from the Mongo database object

            string collectionName = typeof (T).Name.ToLower().Replace("mongo", "");

            MongoCollection = database.GetCollection<T>(collectionName);
        }
    }


    public class BsonMongoConnectionHandler
    {
        public IMongoCollection<BsonDocument> MongoCollection { get; private set; }

        public BsonMongoConnectionHandler(string collectionName)
        {
            var configService = new MongoConfigService();
            InitCollection(configService, collectionName);
        }

        public BsonMongoConnectionHandler(MongoConfigService mongoConfigService, string collectionName)
        {
            InitCollection(mongoConfigService, collectionName);
        }


        private void InitCollection(MongoConfigService mongoConfigService, string collectionName)
        {
             if (mongoConfigService == null)
                throw new ArgumentNullException("mongoConfigService", "mongoConfigService can't be null");

            string url = mongoConfigService.GetConnectionUrl();
            var mongoDbName = mongoConfigService.GetSourceDatabaseName();

            var client = new MongoClient(url);
            IMongoDatabase database = client.GetDatabase(mongoDbName);
            if (database == null)
            {
                throw new DatabaseNotFoundException(mongoDbName);
            }
            
            MongoCollection = database.GetCollection<BsonDocument>(collectionName);
        }
    }
}
