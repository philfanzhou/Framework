using System.Data.Entity;
using System.Data.Entity.SqlServerCompact;

namespace Framework.Infrastructure.Repository.EF.SQLServer
{
    [DbConfigurationType(typeof(SqlCeConfiguration))] 
    public class SqlCeDbContext : DbContext
    {
        public SqlCeDbContext(string fileName)
            : base(fileName)
        {
            
        }
    }

    internal class SqlCeConfiguration : DbConfiguration
    {
        public SqlCeConfiguration()
        {
            SetProviderServices(SqlCeProviderServices.ProviderInvariantName, SqlCeProviderServices.Instance);
        }
    }
}
