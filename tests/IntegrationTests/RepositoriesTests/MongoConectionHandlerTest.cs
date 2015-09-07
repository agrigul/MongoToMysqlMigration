using System;
using Implementation.Entities.Interfaces;
using Implementation.Repositories.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.RepositoriesTests
{
    [TestClass]
    public class MongoConectionHandlerTest
    {
        [TestMethod]
        public void MongoConnectionHandler_void_notNull()
        {
            var mongoConfigService = new MongoConfigService();
            var connectionHandler = new MongoConnectionHandler<MongoBaseEntity>(mongoConfigService);
            Assert.IsNotNull(connectionHandler);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MongoConnectionHandler_nullConfigService_exception()
        {
            new MongoConnectionHandler<MongoBaseEntity>(null);
        }
    }
}
