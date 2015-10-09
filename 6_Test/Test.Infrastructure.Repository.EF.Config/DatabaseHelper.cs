﻿using Framework.Infrastructure.Container;
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
    }
}
