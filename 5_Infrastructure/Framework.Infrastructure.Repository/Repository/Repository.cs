namespace Framework.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;

    public class Repository<TEntity>
        : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        private readonly RepositoryContext _context;

        #endregion

        #region Constructor

        public Repository(IRepositoryContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException("context");
            }

            this._context = context as RepositoryContext;
            if (null == this._context)
            {
                throw new ArgumentException("context");
            }
        }

        #endregion

        #region IRepository Members

        public IUnitOfWork UnitOfWork
        {
            get { return this._context; }
        }

        public virtual void Add(TEntity entity)
        {
            (this._context as IUnitOfWork).RegisterNew(entity);
        }

        public virtual void Update(TEntity entity)
        {
            (this._context as IUnitOfWork).RegisterModified(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            (this._context as IUnitOfWork).RegisterDeleted(entity);
        }

        public virtual bool Exists(ISpecification<TEntity> specification)
        {
            return this._context.Exists(specification);
        }

        public virtual TEntity Get(params object[] keyValues)
        {
            return this._context.Get<TEntity>(keyValues);
        }

        public virtual TEntity Single(ISpecification<TEntity> specification)
        {
            return this._context.Single(specification);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this._context.GetAll<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll(ISpecification<TEntity> specification)
        {
            return this._context.GetAll(specification);
        }

        #endregion
    }
}
