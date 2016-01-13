using Framework.Infrastructure.Repository.EF.SqlServerCe;
using System.Data.Entity;

namespace Test.Infrastructure.Repository.EF.Config
{
    internal class TestSqlCeDbContext : SqlCeDbContext
    {

        public TestSqlCeDbContext()
            : base("FrameworkTest")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AnnouncementConfig());
            modelBuilder.Configurations.Add(new PersonConfig());
        }
    }
}
