using System.Linq;
using Implementation.Entities;
using Implementation.Repositories.Implementation;
using IntegrationTests.RepositoriesTests;
using IntegrationTests.DataForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;


namespace IntegrationTests.TransferTests
{
    [TestClass]
    public class GameTransferTest
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
        public void GameMongoToSql()
        {
            // Arrange 
            string testGameId = MongoJsonTestData.Game.EntityId;
            

            // Act
            var gameMongoRepository = new GenericMongoRepository<GameMongo>();
            var seasonMongoRepository = new GenericMongoRepository<SeasonMongo>();
            GameMongo game = gameMongoRepository.GetByComplexIdAsync(testGameId).Result;

            SeasonMongo season = seasonMongoRepository.GetByComplexIdAsync(game.SeasonId, game.ParentApiId).Result;


            GameSql sqlEntity = EntityFactory.CreateGameSql(game, season);

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<GameSql>(context);
                repo.Insert(sqlEntity);
                repo.Save();
            }


            // Assert
            GameSql sqlSavedEntity;
            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<GameSql>(context);
                sqlSavedEntity = repo.FindWhere(x => x.Id == testGameId).FirstOrDefault();
            }
            Assert.IsNotNull(sqlSavedEntity);
        }

        private void CleanSqlDatabase()
        {
            using (var context = new MySqlDbContext())
            {
                const string tableName = "`test`.`game`";
                string removeQuery = string.Format("DELETE FROM {0} WHERE `Id` NOT like '%{1}%'", tableName, "manual");
                var sqlRepository = new GenericSqlRepository<GameSql>(context);
                sqlRepository.ExecuteQuery(removeQuery);
            }
        }

        private void CleanMongoDataBase()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.DeleteAllTestData("season");
            mongoDirectSirvice.DeleteAllTestData("game");
        }


        private void FillMongoWithData()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Season.Json, "season");
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Game.Json, "game");
        }

    }
}
