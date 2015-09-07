using Implementation.Repositories.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.RepositoriesTests
{
    [TestClass]
    public class MongoConfigServiceTest
    {
        [TestMethod]
        public void MongoConfigService_void_notNull()
        {
            var service = new MongoConfigService();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void GetSourceDatabaseName_void_correctName()
        {
            var service = new MongoConfigService();
            string result = service.GetSourceDatabaseName();
            Assert.AreEqual("testDb", result);
        }


        [TestMethod]
        public void GetConnectionUrl_void_correctUrl()
        {
            var service = new MongoConfigService();
            string result = service.GetConnectionUrl();
            Assert.AreEqual("mongodb://localhost:27017", result);
        }
    }
}
