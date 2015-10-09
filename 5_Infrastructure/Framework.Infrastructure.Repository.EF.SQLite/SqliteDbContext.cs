using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Repository.EF.SQLite
{
    [DbConfigurationType(typeof(SqliteDbConfiguration))]
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(SqliteConnectionConfig dbConnection)
            : base(dbConnection.ToString())
        {
        }
    }

    internal class SqliteDbConfiguration : DbConfiguration
    {
        public SqliteDbConfiguration()
        {
            //SetProviderServices(System.Data.SQLite.EF6.SQLiteProviderFactorySQLiteProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
            SetProviderFactory("SQLite Data Provider", SQLiteProviderFactory.Instance);


            //SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => (IDbExecutionStrategy) new DefaultExecutionStrategy());

            //SetDefaultConnectionFactory(
            //    new SqlConnectionFactory(
            //       @"Server=(localdb)\projects; Database=UserContextDb;Trusted_Connection=true"));
        }
    }
}
