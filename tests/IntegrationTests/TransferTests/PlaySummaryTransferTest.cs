using System.Linq;
using Implementation.Entities;
using Implementation.Repositories.Implementation;
using IntegrationTests.RepositoriesTests;
using IntegrationTests.DataForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace IntegrationTests.TransferTests
{

    [TestClass]
    public class PlaySummaryTransferTest
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
        public void PlaySummaryMongoToSql()
        {
            // Arrange 
            string testId = MongoJsonTestData.Play.EntityId;
            

            // Act
            var playRepository = new GenericMongoRepository<PlayMongo>();
            
            PlayMongo entity = playRepository.GetByComplexIdAsync(testId, MongoJsonTestData.Play.ParentApiId).Result;

            var sqlEntity = EntityFactory.CreatePlaySummarySql(entity);

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlaySummarySql>(context);
                repo.Insert(sqlEntity);
                repo.Save();
            }


            // Assert
            PlaySummarySql sqlSavedEntity;
            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<PlaySummarySql>(context);
                sqlSavedEntity = repo.FindWhere(x => x.Id == testId).FirstOrDefault();
            }
            Assert.IsNotNull(sqlSavedEntity);
        }

        private void CleanSqlDatabase()
        {
            using (var context = new MySqlDbContext())
            {
                const string tableName = "`test`.`playsummary`";
                string removeQuery = string.Format("DELETE FROM {0} WHERE `Id` NOT like '%{1}%'", tableName, "manual");
                var sqlRepository = new GenericSqlRepository<TeamSql>(context);
                sqlRepository.ExecuteQuery(removeQuery);
            }
        }

        private void CleanMongoDataBase()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.DeleteAllTestData("play");
        }


        private void FillMongoWithData()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Play.Json, "play");
        }

    }
}
