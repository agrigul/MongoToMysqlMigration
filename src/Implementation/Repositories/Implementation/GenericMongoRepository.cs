using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Implementation.Entities.Interfaces;
using Implementation.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Implementation.Repositories.Implementation
{
    /// <summary>
    /// Implementation of interface IMongoRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericMongoRepository<T> : IGenericMongoRepository<T> where T : IMongoEntity
    {
        protected readonly MongoConnectionHandler<T> MongoConnectionHandler;

        public GenericMongoRepository()
        {
            var configService = new MongoConfigService();
            MongoConnectionHandler = new MongoConnectionHandler<T>(configService);
        }


        public virtual async Task<IList<T>> FindByFilter(FilterDefinition<T> filter)
        {
            Task<List<T>> res = MongoConnectionHandler.MongoCollection.Find(filter).ToListAsync();
            return await res;
        }


        public virtual async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(x => x.IdInMongo, id);
            Task<T> res = MongoConnectionHandler.MongoCollection.Find(filter).SingleOrDefaultAsync();
            return await res;
        }

        public virtual async Task<T> GetByComplexIdAsync(string entityId, string parentApiId = null)
        {
            // var filter = Builders<T>.Filter.Eq(x => x.Id, new ObjectId(id));
            var filter = Builders<T>.Filter.Eq(x => x.EntityId, entityId);
            
            if(string.IsNullOrEmpty(parentApiId) == false)
                filter = filter & Builders<T>.Filter.Eq(x => x.ParentApiId, parentApiId);

            Task<T> res = MongoConnectionHandler.MongoCollection.Find(filter).SingleOrDefaultAsync();
            return await res;
        }


        public virtual async Task<List<T>> GetAllAsync()
        {
            IFindFluent<T, T> documents = MongoConnectionHandler.MongoCollection.Find(new BsonDocument());
            var result = documents.ToListAsync();
            return await result;
        }


        public virtual List<T> GetAll()
        {
            IFindFluent<T, T> documents = MongoConnectionHandler.MongoCollection.Find(new BsonDocument());
            var result = documents.ToListAsync();
            result.Wait();
            return result.Result;
        }


        public virtual async void InsertAsync(T entity)
        {
            await MongoConnectionHandler.MongoCollection.InsertOneAsync(entity);
        }

        public virtual async void DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "can't delete null entity");

            await MongoConnectionHandler.MongoCollection.DeleteOneAsync(x => x.IdInMongo == entity.IdInMongo);
        }


        public virtual void Insert(T entity)
        {
            MongoConnectionHandler.MongoCollection.InsertOneAsync(entity).Wait();
        }


        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity", "can't delete null entity");

            MongoConnectionHandler.MongoCollection.DeleteOneAsync(x => x.IdInMongo == entity.IdInMongo).Wait();
        }


    }
}