using Framework.Infrastructure.Container;
using Framework.Infrastructure.Repository;
using Framework.Infrastructure.Repository.EF;

namespace Test.Infrastructure.Repository.EF.Config
{
    public static class DatabaseHelper
    {
        public static void InitializeSqlServer(bool clearDatabase)
        {
            ContainerHelper.Instance
                .RegisterType<IRepositoryContext,
                EntityFrameworkRepositoryContext<TestSqlServerDbContext>>();

            if (clearDatabase)
            {
                TestSqlServerDbContext dbContext = new TestSqlServerDbContext();
                dbContext.Database.Delete();
                dbContext.Database.Create();
            }
        }

        public static void InitializeSqlCe(bool clearDatabase)
        {
            ContainerHelper.Instance
                .RegisterType<IRepositoryContext,
                EntityFrameworkRepositoryContext<TestSqlCeDbContext>>();

            if (clearDatabase)
            {
                TestSqlCeDbContext dbContext = new TestSqlCeDbContext();
                dbContext.Database.Delete();
                dbContext.Database.Create();
            }
        }

        public static void InitializeSqlite(bool clearDatabase)
        {
            ContainerHelper.Instance
                .RegisterType<IRepositoryContext,
                EntityFrameworkRepositoryContext<TestSQLiteDbContext>>();
        }

        public static void InitializeMySql(bool clearDatabase)
        {
            ContainerHelper.Instance
                .RegisterType<IRepositoryContext,
                EntityFrameworkRepositoryContext<TestMySqlDbContext>>();

            if (clearDatabase)
            {
                TestMySqlDbContext dbContext = new TestMySqlDbContext();
                dbContext.Database.Delete();
                dbContext.Database.Create();
            }
        }
    }
}
