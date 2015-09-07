using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Implementation.Entities;
using Implementation.Repositories.Implementation;
using IntegrationTests.RepositoriesTests;
using IntegrationTests.DataForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;


namespace IntegrationTests.TransferTests
{

    [TestClass]
    public class PlayPlayerPartisipantsTransferTest
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
        public void PlayPlayerPartisipantsMontoToSql()
        {
            // Arrange 
            string testId = MongoJsonTestData.Play.EntityId;


            // Act
            var playRepository = new GenericMongoRepository<PlayMongo>();
            var playerRepository = new GenericMongoRepository<PlayerMongo>();

            PlayMongo play = playRepository.GetByComplexIdAsync(testId, MongoJsonTestData.Play.ParentApiId).Result;


            var filter = Builders<PlayerMongo>.Filter.Eq(x => x.PlayId, play.EntityId) &
                         Builders<PlayerMongo>.Filter.Eq(x => x.ParentApiId, play.ParentApiId);


            IList<PlayerMongo> players = playerRepository.FindByFilter(filter).Result;


            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlayPlayerPartisipantsSql>(context);

                IList<PlayPlayerPartisipantsSql> sqlEntity = EntityFactory.CreatePlayPlayerPartisipantsSql(play, players);
                foreach (var item in sqlEntity)
                {
                    repo.Insert(item);
                }
                
                repo.Save();
            }


            // Assert
            IList<PlayPlayerPartisipantsSql> sqlSavedEntity;
            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlayPlayerPartisipantsSql>(context);
                sqlSavedEntity = repo.FindWhere(x => x.PlayId == play.EntityId).ToList();
            }
            Assert.IsNotNull(sqlSavedEntity);
            Assert.AreEqual(2, sqlSavedEntity.Count());
        }

        private void CleanSqlDatabase()
        {
            using (var context = new MySqlDbContext())
            {
                const string tableName = "`test`.`playplayerparticipants`";
                string removeQuery = string.Format("DELETE FROM {0} WHERE `Id` NOT like '%{1}%'", tableName, "manual");
                var sqlRepository = new GenericSqlRepository<TeamSql>(context);
                sqlRepository.ExecuteQuery(removeQuery);
            }
        }

        private void CleanMongoDataBase()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.DeleteAllTestData("play");
            mongoDirectSirvice.DeleteAllTestData("player");
        }


        private void FillMongoWithData()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Play.Json, "play");
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.PlayerPartisipant1.Json, "player");
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.PlayerPartisipant2.Json, "player");
        }

    }
}
