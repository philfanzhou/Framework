namespace Framework.Infrastructure.Repository
{
    using System;

    public interface IRepositoryContext : IDisposable
    {
        Guid UUID { get; }
    }
}
