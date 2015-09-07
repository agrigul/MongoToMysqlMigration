using Implementation.Entities.Interfaces;
using Implementation.Repositories.Implementation;
using IntegrationTests.DataForTests;

namespace IntegrationTests.RepositoriesTests.Mongo
{
    public class TestDataForMongoPreparationService
    {
        public string MongoDBName { get; set; }


        public TestDataForMongoPreparationService() { }
        public TestDataForMongoPreparationService(string mongoDbName)
        {
            MongoDBName = mongoDbName;
        }


        public void RemoveData()
        {
            var service = new MongoDbDirectManipulationsService();
            service.DeleteAllTestData(MongoDBName);
        }

        public T ArrangeAndAct<T>(TestData testData) where T : MongoBaseEntity
        {
            Arrange<T>(testData);
            return Act<T>(testData);
        }

        public void Arrange<T>(TestData testData) where T : MongoBaseEntity
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.InsertJsonToMongo(testData.Json, MongoDBName);
        }


        public T Act<T>(TestData testData) where T : MongoBaseEntity
        {
            var repository = new GenericMongoRepository<T>();
            var entity = repository.GetByIdAsync(testData.Id).Result;

            return entity;
        }
    }
}