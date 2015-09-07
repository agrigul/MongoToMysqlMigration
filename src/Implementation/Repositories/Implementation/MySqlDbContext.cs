using System.Data.Entity;
using Implementation.Entities;

namespace Implementation.Repositories.Implementation
{
    /// <summary>
    /// Connects to MySQL database and sets the mappings
    /// </summary>
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext()
            : base("MySqlDbConnectionString")
        {
            Database.SetInitializer<MySqlDbContext>(new MySqlDbInitializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MapGameSQL());
            modelBuilder.Configurations.Add(new MapTeamSQL());
            modelBuilder.Configurations.Add(new MapPayerSql());
            modelBuilder.Configurations.Add(new MapPlaySummarySql());
            modelBuilder.Configurations.Add(new MapPlayPlayerPartisipantsSql());
            base.OnModelCreating(modelBuilder);
        }
    }


    public class MySqlDbInitializer : CreateDatabaseIfNotExists<MySqlDbContext>
    {
        protected override void Seed(MySqlDbContext context)
        {
            base.Seed(context);
        }
    }
}
