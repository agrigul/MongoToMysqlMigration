using System.Collections.Generic;
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
    public class TeamTransferTest
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
        public void TeamMongoToSql()
        {
            // Arrange 
            string testId = MongoJsonTestData.Team.EntityId;
            

            // Act
            var teamMongoRepository = new GenericMongoRepository<TeamMongo>();
            var conferenceMongoRepository = new GenericMongoRepository<ConferenceMongo>();
            var divisionMongoRepository = new GenericMongoRepository<DivisionMongo>();
            TeamMongo team = teamMongoRepository.GetByComplexIdAsync(testId, MongoJsonTestData.Team.ParentApiId).Result;

            var conferenceFilter = Builders<ConferenceMongo>.Filter.Eq(x => x.EntityId, team.ConvferenceId) & 
                                   Builders<ConferenceMongo>.Filter.Eq(x => x.ParentApiId, team.ParentApiId);
            IList<ConferenceMongo> conference = conferenceMongoRepository.FindByFilter(conferenceFilter).Result;


            var divisionFilter = Builders<DivisionMongo>.Filter.Eq(x => x.EntityId, team.DivisionId) &
                                 Builders<DivisionMongo>.Filter.Eq(x => x.ParentApiId, team.ParentApiId);

            IList<DivisionMongo> division = divisionMongoRepository.FindByFilter(divisionFilter).Result;


            var sqlEntity = EntityFactory.CreateTeamSql(team, conference[0], division[0]);

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<TeamSql>(context);
                repo.Insert(sqlEntity);
                repo.Save();
            }


            // Assert
            TeamSql sqlSavedEntity;
            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<TeamSql>(context);
                sqlSavedEntity = repo.FindWhere(x => x.Id == testId).FirstOrDefault();
            }
            Assert.IsNotNull(sqlSavedEntity);
        }

        private void CleanSqlDatabase()
        {
            using (var context = new MySqlDbContext())
            {
                const string tableName = "`test`.`team`";
                string removeQuery = string.Format("DELETE FROM {0} WHERE `Id` NOT like '%{1}%'", tableName, "manual");
                var sqlRepository = new GenericSqlRepository<TeamSql>(context);
                sqlRepository.ExecuteQuery(removeQuery);
            }
        }

        private void CleanMongoDataBase()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.DeleteAllTestData("team");
            mongoDirectSirvice.DeleteAllTestData("conference");
            mongoDirectSirvice.DeleteAllTestData("division");
        }


        private void FillMongoWithData()
        {
            var mongoDirectSirvice = new MongoDbDirectManipulationsService();
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Team.Json, "team");
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Conference.Json, "conference");
            mongoDirectSirvice.InsertJsonToMongo(MongoJsonTestData.Division.Json, "division");
        }

    }
}
