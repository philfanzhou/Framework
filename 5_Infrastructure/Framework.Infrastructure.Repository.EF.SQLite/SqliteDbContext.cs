using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Reflection;

namespace Framework.Infrastructure.Repository.EF.SQLite
{
    [DbConfigurationType(typeof(SQLiteDbConfiguration))]
    public class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext(SQLiteConnectionConfig dbConnection)
            : base(dbConnection.ToString())
        {
            //数据库不存在时重新创建数据库
            //Database.SetInitializer<SQLiteDbContext>(new CreateDatabaseIfNotExists<SQLiteDbContext>());
        }
    }

    internal class SQLiteDbConfiguration : DbConfiguration
    {
        public SQLiteDbConfiguration()
        {
            /*
      <remove invariant="System.Data.SQLite.EF6" />
      <add name="SQLite Data Provider (Entity Framework 6)" 
            invariant="System.Data.SQLite.EF6" 
            type="System.Data.SQLite.EF6.SQLiteProviderFactory, System.Data.SQLite.EF6" />
      
      <remove invariant="System.Data.SQLite" />
      <add name="SQLite Data Provider" 
            invariant="System.Data.SQLite" 
            description=".NET Framework Data Provider for SQLite" 
            type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite" />
            */

            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

            Type t = Type.GetType(
                       "System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
            FieldInfo fi = t.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            SetProviderServices("SQLite Data Provider (Entity Framework 6)", (DbProviderServices)fi.GetValue(null));
        }
    }
}
