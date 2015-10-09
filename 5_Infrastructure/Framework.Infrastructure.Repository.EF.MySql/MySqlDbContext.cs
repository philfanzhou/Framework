using MySql.Data.Entity;
using System.Data.Entity;

namespace Framework.Infrastructure.Repository.EF.SQLServer
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))] 
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(MySqlConnectionConfig dbConnection)
            : base(dbConnection.ToString())
        {
        }
    }
}
