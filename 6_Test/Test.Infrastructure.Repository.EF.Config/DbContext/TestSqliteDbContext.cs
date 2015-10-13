using Framework.Infrastructure.Repository.EF.SQLite;
using System.Data.Entity;

namespace Test.Infrastructure.Repository.EF.Config
{
    internal class TestSQLiteDbContext : SQLiteDbContext
    {
        private static readonly SQLiteConnectionConfig connection = new SQLiteConnectionConfig
        {
            //"data source = " + 
            Database = System.Environment.CurrentDirectory + @"\FrameworkTest.db",
        };

        public TestSQLiteDbContext()
            : base(connection)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<TestSQLiteDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AnnouncementConfig());
            modelBuilder.Configurations.Add(new PersonConfig());
        }
    }
}
