﻿using System.Collections.Generic;

namespace Framework.Infrastructure.Repository
{
    /// <summary>
    /// Contract for ‘UnitOfWork pattern’. For more
    /// related info see http://martinfowler.com/eaaCatalog/unitOfWork.html or
    /// http://msdn.microsoft.com/en-us/magazine/dd882510.aspx
    /// </summary>
    public interface IUnitOfWork
    {
        void RegisterNew<TEntity>(TEntity item)
            where TEntity : class;

        void RegisterNew<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class;

        void RegisterModified<TEntity>(TEntity item)
            where TEntity : class;

        void RegisterDeleted<TEntity>(TEntity item)
            where TEntity : class;

        void Commit();

        void Rollback();
    }
}
