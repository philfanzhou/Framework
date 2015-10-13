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
            //SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

            Type t = Type.GetType(
                       "System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
            FieldInfo fi = t.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)fi.GetValue(null));
        }
    }
}
