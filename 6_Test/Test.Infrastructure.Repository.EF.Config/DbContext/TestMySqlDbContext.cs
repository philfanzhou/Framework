using Framework.Infrastructure.Repository.EF.SQLServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Infrastructure.Repository.EF.Config
{
    internal class TestMySqlDbContext : MySqlDbContext
    {
        private static readonly MySqlConnectionConfig connection = new MySqlConnectionConfig
        {
            Server = "localhost",
            Port = "3306",
            Database = "FrameworkTest",
            Uid = "root",
            Password = "123456"
        };

        public TestMySqlDbContext()
            : base(connection)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AnnouncementConfig());
            modelBuilder.Configurations.Add(new PersonConfig());
        }
    }
}
