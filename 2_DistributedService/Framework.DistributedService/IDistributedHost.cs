using System;
using System.ServiceModel;

namespace Framework.DistributedService
{
    public interface IDistributedHost : ICommunicationObject, IDisposable
    {
        string Name { get; }
    }
}
