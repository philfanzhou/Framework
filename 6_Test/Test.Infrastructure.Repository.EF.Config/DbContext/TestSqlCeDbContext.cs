using Framework.Infrastructure.Repository.EF.SQLServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
