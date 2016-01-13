using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Framework.Infrastructure.Repository.EF.SqlServer
{
    [DbConfigurationType(typeof(SqlServerDbConfiguration))] 
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(SqlServerConnectionConfig dbConnection)
            : base(dbConnection.ToString())
        {
        }
    }

    internal class SqlServerDbConfiguration : DbConfiguration
    {
        public SqlServerDbConfiguration()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
            //SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => (IDbExecutionStrategy) new DefaultExecutionStrategy());

            //SetDefaultConnectionFactory(
            //    new SqlConnectionFactory(
            //       @"Server=(localdb)\projects; Database=UserContextDb;Trusted_Connection=true"));
        }
    }
}
