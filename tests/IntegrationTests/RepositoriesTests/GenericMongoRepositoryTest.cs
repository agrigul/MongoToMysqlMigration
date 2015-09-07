using System.Collections.Generic;
using System.Linq;
using Implementation.Entities;
using Implementation.Entities.Interfaces;
using Implementation.Repositories.Implementation;
using IntegrationTests.DataForTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace IntegrationTests.RepositoriesTests
{
    [TestClass]
    public class GenericMongoRepositoryTest
    {
        [TestInitialize]
        public void RemoveData()
        {
            var service = new MongoDbDirectManipulationsService();
            service.DeleteAllTestData("game");
        }

        [TestMethod]
        public void GenericMongoRepository_void_notNull()
        {
            var repository = new GenericMongoRepository<IMongoEntity>();
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void GetAll_void_entities()
        {
            CreateMongoData();
            var repository = new GenericMongoRepository<GameMongo>();
            List<GameMongo> gameList = repository.GetAll();
            Assert.IsNotNull(gameList);
            Assert.IsTrue(gameList.Any());
        }

        [TestMethod]
        public void GetAllAsync_void_entities()
        {
            CreateMongoData();
            var repository = new GenericMongoRepository<GameMongo>();
            List<GameMongo> gameList = repository.GetAllAsync().Result;
            Assert.IsNotNull(gameList);
            Assert.IsTrue(gameList.Any());
        }


        [TestMethod]
        public void GetByIdAsync_void_entities()
        {
            CreateMongoData();
            var repository = new GenericMongoRepository<GameMongo>();
            GameMongo game = repository.GetByIdAsync(MongoJsonTestData.Game.Id).Result;
            Assert.IsNotNull(game);
        }


        [TestMethod]
        public void GetByComplexIdAsync_void_entities()
        {
            CreateMongoData();
            var repository = new GenericMongoRepository<GameMongo>();
            GameMongo game = repository.GetByComplexIdAsync(MongoJsonTestData.Game.EntityId, "schedule").Result;
            Assert.IsNotNull(game);
        }


        [TestMethod]
        public void Insert_void_entities()
        {            
            var repository = new GenericMongoRepository<GameMongo>();
            GameMongo entity = new GameMongo
            {
                IdInMongo = MongoJsonTestData.Game.Id,
                AwayId = "testAwayId",
                HomeId = "testHomeId"
            };

            repository.Insert(entity);

            GameMongo savedEntity = repository.GetByIdAsync(entity.IdInMongo).Result;
            Assert.IsNotNull(savedEntity);
            Assert.AreEqual(savedEntity.AwayId, entity.AwayId);
            Assert.AreEqual(savedEntity.HomeId, entity.HomeId);
        }


        [TestMethod]
        public void FindByFilter_filter_entity()
        {
            CreateMongoData();
            var repository = new GenericMongoRepository<GameMongo>();
            var filter = Builders<GameMongo>.Filter.Eq(x => x.SeasonId, "http://api.sportsdatallc.org/nfl-t1/2014/REG/schedule.xml");
            IList<GameMongo> game = repository.FindByFilter(filter).Result;
            Assert.IsNotNull(game);
            Assert.IsTrue(game.Any());
        }


        private void CreateMongoData()
        {
            var service = new MongoDbDirectManipulationsService();
            service.InsertJsonToMongo(MongoJsonTestData.Game.Json, "game");
        }
    }
}
