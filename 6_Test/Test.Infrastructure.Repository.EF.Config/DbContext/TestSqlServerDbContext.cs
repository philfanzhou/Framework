using Framework.Infrastructure.Repository.EF.SqlServer;
using System.Data.Entity;

namespace Test.Infrastructure.Repository.EF.Config
{
    internal class TestSqlServerDbContext : SqlServerDbContext
    {
        private static readonly SqlServerConnectionConfig connection = new SqlServerConnectionConfig
        {
            Server = @"(localdb)\projects",
            Database = "FrameworkTest",
            TrustedConnection = true
        };

        public TestSqlServerDbContext()
            : base(connection)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AnnouncementConfig());
            modelBuilder.Configurations.Add(new PersonConfig());
        }
    }
}
