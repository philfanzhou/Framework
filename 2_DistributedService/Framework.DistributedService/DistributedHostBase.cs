using System;
using System.ServiceModel;

namespace Framework.DistributedService
{
    public abstract class DistributedHostBase : ServiceHost, IDistributedHost
    {
        public abstract string Name
        {
            get;
        }

        protected DistributedHostBase(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) { }
    }
}
