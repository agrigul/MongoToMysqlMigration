using System.Linq;
using Implementation.Entities;
using IntegrationTests.DataForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace IntegrationTests.RepositoriesTests.Mongo
{
    [TestClass]
    public class MongoEntityMappingsTests
    {

        [TestMethod]
        public void GameMapping_id_allProperiesSet()
        {
            const string mongoDbName = "game";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<GameMongo>(MongoJsonTestData.Game);


            Assert.IsNotNull(entity);
            Assert.AreEqual(1432330350488, entity.DdUpdatedId);
            Assert.AreEqual("schedule", entity.ParentApiId);
            Assert.AreEqual("GB", entity.AwayId);
            Assert.AreEqual("SEA", entity.HomeId);
            Assert.AreEqual("http://api.sportsdatallc.org/nfl-t1/2014/REG/schedule.xml", entity.SeasonId);

        }

        [TestMethod]
        public void SeasonMapping_id_allProperiesSet()
        {
            const string mongoDbName = "season";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<SeasonMongo>(MongoJsonTestData.Season);

            Assert.IsNotNull(entity);
            Assert.AreEqual(1432330350488, entity.DdUpdatedId);
            Assert.AreEqual("schedule", entity.ParentApiId);
            Assert.AreEqual(2014, entity.Season);
            Assert.AreEqual("PRE", entity.SeasonType);
            Assert.AreEqual("NO", entity.Team);
            Assert.IsNotNull(entity.Weeks);
            Assert.IsNotNull(entity.Weeks.Any());
            Assert.IsNotNull(entity.Weeks[0].Week);
            Assert.AreEqual(entity.Weeks[0].Week.WeekValue, 1);
            Assert.AreEqual(entity.Weeks[1].Week.WeekValue, 16);
        }

        [TestMethod]
        public void TeamMapping_id_allProperiesSet()
        {
            const string mongoDbName = "team";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<TeamMongo>(MongoJsonTestData.Team);

            Assert.IsNotNull(entity);
            Assert.AreEqual("BUF", entity.EntityId);
            Assert.AreEqual(1432330620462, entity.DdUpdatedId);
            Assert.AreEqual("hierarchy", entity.ParentApiId);
            Assert.AreEqual("Bills", entity.Name);
            Assert.AreEqual("AFC", entity.ConvferenceId);
            Assert.AreEqual("AFC_EAST", entity.DivisionId);

        }

        [TestMethod]
        public void ConferenceMapping_id_allProperiesSet()
        {
            const string mongoDbName = "conference";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<ConferenceMongo>(MongoJsonTestData.Conference);

            Assert.IsNotNull(entity);
            Assert.AreEqual("AFC", entity.EntityId);
            Assert.AreEqual(1432330620462, entity.DdUpdatedId);
            Assert.AreEqual("hierarchy", entity.ParentApiId);
            Assert.AreEqual("AFC", entity.Name);
            Assert.IsTrue(entity.Divisions.Any());
            Assert.IsNotNull(entity.Divisions.Single(x => x.divisionId == "AFC_EAST"));
        }

        [TestMethod]
        public void DivisionMapping_id_allProperiesSet()
        {
            const string mongoDbName = "division";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<DivisionMongo>(MongoJsonTestData.Division);

            Assert.IsNotNull(entity);
            Assert.AreEqual("AFC_EAST", entity.EntityId);
            Assert.AreEqual(1432330620462, entity.DdUpdatedId);
            Assert.AreEqual("hierarchy", entity.ParentApiId);
            Assert.AreEqual("AFC East", entity.Name);
            Assert.AreEqual("AFC", entity.ConferenceId);
            Assert.IsTrue(entity.Teams.Any());
            Assert.IsNotNull(entity.Teams.Single(x => x.teamId == "BUF"));

        }
        
        [TestMethod]
        public void Player1Mapping_id_allProperiesSet()
        {
            const string mongoDbName = "player";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<PlayerMongo>(MongoJsonTestData.Player1);

            Assert.IsNotNull(entity);
            Assert.AreEqual("64d9a11b-2d05-4173-ac72-4f9e63fb4aa6", entity.EntityId);
            Assert.AreEqual(1432824161195, entity.DdUpdatedId);
            Assert.AreEqual("pbp", entity.ParentApiId);
            Assert.AreEqual("C.J. Spiller", entity.FullName);
            Assert.AreEqual("RB", entity.Position);
            Assert.AreEqual("BUF", entity.TeamId);
        }
        
        [TestMethod]
        public void Player2Mapping_id_allProperiesSet()
        {
            const string mongoDbName = "player";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<PlayerMongo>(MongoJsonTestData.Player2);

            Assert.IsNotNull(entity);
            Assert.AreEqual("5f3cc875-e802-46b2-81ad-3ffb7a3a1662", entity.EntityId);
            Assert.AreEqual(1432328610564, entity.DdUpdatedId);
            Assert.AreEqual("depthchart", entity.ParentApiId);
            Assert.AreEqual("Michael Palardy", entity.FullName);
            Assert.AreEqual("K", entity.Position);
            Assert.AreEqual(1, entity.Depth);
            Assert.AreEqual("ACT", entity.Status);
            Assert.AreEqual("STL", entity.TeamId);
        }

        [TestMethod]
        public void PlayerPartisipantMapping_id_allProperiesSet()
        {
            const string mongoDbName = "player";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<PlayerMongo>(MongoJsonTestData.PlayerPartisipant1);

            Assert.IsNotNull(entity);
            Assert.IsFalse(string.IsNullOrEmpty(entity.GameId));
            Assert.IsFalse(string.IsNullOrEmpty(entity.PlayId));
        }


        [TestMethod]
        public void PlayMapping_id_allProperiesSet()
        {
            const string mongoDbName = "play";
            var service = new TestDataForMongoPreparationService(mongoDbName);
            service.RemoveData();

            var entity = service.ArrangeAndAct<PlayMongo>(MongoJsonTestData.Play);

            Assert.IsNotNull(entity);
            Assert.AreEqual("86d7d20c-e031-449d-bf60-3ebe623123b1", entity.EntityId);
            Assert.AreEqual("pbp", entity.ParentApiId);
            Assert.AreEqual("3c42f4ea-e4b3-449d-82d5-36850144add9", entity.GameId);
            Assert.AreEqual("kick", entity.Type);
            Assert.AreEqual(1, entity.Down);
        }
    }
}
