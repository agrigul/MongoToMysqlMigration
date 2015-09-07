using System;
using System.Collections.Generic;
using System.Linq;
using Implementation.Entities;
using Implementation.Repositories.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.RepositoriesTests
{
    public abstract class CRUDOperationsTestsBase
    {
        public void ExecuteQuery(string SqlQuery)
        {
            using (var dbContext = new MySqlDbContext())
            {
                dbContext.Database.ExecuteSqlCommand(SqlQuery);
                dbContext.SaveChanges();
            }
        }
    }

    [TestClass]
    public class GenericSqlRepositoryTest : CRUDOperationsTestsBase
    {
        private const string testId = "testId";
        private readonly string TableName = "`test`.`game`";


        private void InsertNewGameItem(int idNumber = 0)
        {
            string addItem = string.Format("INSERT INTO {0} (`Id`,`GameDate`) VALUES ('{1}',NOW());", TableName, testId + idNumber);
            ExecuteQuery(addItem);
        }

        [TestInitialize]
        public void RemoveData()
        {
            string removeQuery = string.Format("DELETE FROM {0} WHERE `Id` NOT like '%{1}%'", TableName, "manual");
            ExecuteQuery(removeQuery);
        }

        [TestMethod]
        public void GetAll_void_notEmpty()
        {
            InsertNewGameItem(1);

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<GameSql>(context);
                IList<GameSql> result = repo.GetAll();
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Any());
            }
        }



        [TestMethod]
        public void Insert_item_itemSaved()
        {
            const string id = "testInsert";
            GameSql newEntity = new GameSql()
            {
                Id = id,
                GameDate = DateTime.Now,
                Season = 1,
                SeasonType = "",
                Week = 1,
                HomeTeamId = "",
                AwayTeamId = "",
                WeatherCondition = "",
                HomeTeamQ1Score = 1,
                HomeTeamQ2Score = 1,
                HomeTeamQ3Score = 1,
                HomeTeamQ4Score = 1,
                AwayTeamQ1Score = 1,
                AwayTeamQ2Score = 1,
                AwayTeamQ3Score = 1,
                AwayTeamQ4Score = 1,
                HomeTeamFinalScore = 1,
                AwayTeamFinalScore = 1,
            };


            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<GameSql>(context);
                repo.Insert(newEntity);
                repo.Save();
            }

            using (var context = new MySqlDbContext())
            {
                var repo = new GenericSqlRepository<GameSql>(context);
                GameSql result = repo.FindWhere(x => x.Id == id).SingleOrDefault();
                Assert.IsNotNull(result);
            }
        }
    }
}
