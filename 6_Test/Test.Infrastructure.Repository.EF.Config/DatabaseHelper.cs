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
                .RegisterType<RepositoryContext,
                EntityFrameworkRepositoryContext<TestSqlServerDbContext>>();

            if (clearDatabase)
            {
                TestSqlServerDbContext dbContext = new TestSqlServerDbContext();
                dbContext.Database.Delete();
                dbContext.Database.Create();
            }
        }

        public static void InitializeSqlite(bool clearDatabase)
        {
            ContainerHelper.Instance
                .RegisterType<RepositoryContext,
                EntityFrameworkRepositoryContext<TestSqliteDbContext>>();
        }

        public static void InitializeMySql(bool clearDatabase)
        {
            ContainerHelper.Instance
                .RegisterType<RepositoryContext,
                EntityFrameworkRepositoryContext<TestSqlServerDbContext>>();

            if (clearDatabase)
            {
                TestSqlServerDbContext dbContext = new TestSqlServerDbContext();
                dbContext.Database.Delete();
                dbContext.Database.Create();
            }
        }
    }
}
