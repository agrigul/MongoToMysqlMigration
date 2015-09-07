using System.Linq;
using Implementation.Entities;
using Implementation.Repositories.Implementation;
using IntegrationTests.RepositoriesTests;
using IntegrationTests.DataForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace IntegrationTests.TransferTests
{

    [TestClass]
    public class PlayerTransferTest
    {
        readonly SqlEntitiesFactory EntityFactory = new SqlEntitiesFactory();

        [TestInitialize]
        public void BeforeTest()
        {
            CleanMongoDataBase();
            CleanSqlDatabase();

            FillMongoWithData();
        }

        [TestMethod]
        public void Player1MongoToSql()
        {
            // Arrange 
            string testId = MongoJsonTestData.Player1.EntityId;
            

            // Act
            var playerMongoRepository = new GenericMongoRepository<PlayerMongo>();
            PlayerMongo player = playerMongoRepository.GetByComplexIdAsync(testId, MongoJsonTestData.Player1.ParentApiId).Result;

            var sqlEntity = EntityFactory.CreatePlayerSql(player);

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlayerSql>(context);
                repo.Insert(sqlEntity);
                repo.Save();
            }


            // Assert
            PlayerSql sqlSavedEntity;
            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlayerSql>(context);
                sqlSavedEntity = repo.FindWhere(x => x.Id == testId).FirstOrDefault();
            }
            Assert.IsNotNull(sqlSavedEntity);
            Assert.AreEqual("C.J. Spiller", sqlSavedEntity.PlayerFullName);
            Assert.AreEqual("RB", sqlSavedEntity.Position);
            Assert.AreEqual("BUF", sqlSavedEntity.CurrentTeamId);
            Assert.IsNull(sqlSavedEntity.CurrentStatus);
            Assert.IsNull(sqlSavedEntity.CurrentDepthNumber);
            
        }


        [TestMethod]
        public void Player2MongoToSql()
        {
            // Arrange 
            string testId = MongoJsonTestData.Player2.EntityId;


            // Act
            var playerMongoRepository = new GenericMongoRepository<PlayerMongo>();
            PlayerMongo player = playerMongoRepository.GetByComplexIdAsync(testId, MongoJsonTestData.Player2.ParentApiId).Result;

            var sqlEntity = EntityFactory.CreatePlayerSql(player);

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlayerSql>(context);
                repo.Insert(sqlEntity);
                repo.Save();
            }


            // Assert
            PlayerSql sqlSavedEntity;
            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlayerSql>(context);
                sqlSavedEntity = repo.FindWhere(x => x.Id == testId).FirstOrDefault();
            }
            Assert.IsNotNull(sqlSavedEntity);
            Assert.AreEqual("Michael Palardy", sqlSavedEntity.PlayerFullName);
            Assert.AreEqual("K", sqlSavedEntity.Position);
            Assert.AreEqual("STL", sqlSavedEntity.CurrentTeamId);
            Assert.AreEqual(1, sqlSavedEntity.CurrentDepthNumber);
            Assert.AreEqual("ACT", sqlSavedEntity.CurrentStatus);
        }

        private void CleanSqlDatabase()
        {
            using (var context = new MySqlDbContext())
            {
                const string tableName = "`test`.`player`";
                string removeQuery = string.Format("DELETE FROM {0} WHERE `Id` NOT like '%{1}%'", tableName, "manual");
                var sqlRepository = new GenericSqlRepository<TeamSql>(context);
                sqlRepository.ExecuteQuery(removeQuery);
            }
        }

        private void CleanMongoDataBase()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.DeleteAllTestData("player");
        }


        private void FillMongoWithData()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Player1.Json, "player");
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Player2.Json, "player");
        }

    }
}
