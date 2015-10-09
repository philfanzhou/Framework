using Framework.Infrastructure.Repository.EF.SQLite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Infrastructure.Repository.EF.Config
{
    internal class TestSqliteDbContext : SqliteDbContext
    {
        private static readonly SqliteConnectionConfig connection = new SqliteConnectionConfig
        {
            Database = "FrameworkTest.db",
        };

        public TestSqliteDbContext()
            : base(connection)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<TestSqliteDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AnnouncementConfig());
            modelBuilder.Configurations.Add(new PersonConfig());
        }
    }
}
