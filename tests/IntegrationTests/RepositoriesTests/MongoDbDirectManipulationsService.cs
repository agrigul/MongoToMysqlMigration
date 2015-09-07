using Implementation.Repositories.Implementation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntegrationTests.RepositoriesTests
{
    public class MongoDbDirectManipulationsService
    {

        public void InsertJsonToMongo(string json, string collectionName)
        {
            BsonDocument document
                = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);

            var mongoConnectionHandler = new BsonMongoConnectionHandler(collectionName);
            mongoConnectionHandler.MongoCollection.InsertOneAsync(document).Wait();
        }


        public void DeleteAllTestData(string collectionName)
        {
            var mongoConnectionHandler = new BsonMongoConnectionHandler(collectionName);
            // delete all items with ids doesn't contain 'manual'
            mongoConnectionHandler.MongoCollection.DeleteManyAsync(new JsonFilterDefinition<BsonDocument>("{'_id' : {'$not': /.*manual*./}}")).Wait();
            
        }
    }
}
