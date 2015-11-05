using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Framework.DistributedService
{
    public class WebHttpHost<TContract, TService> : ServiceHost, IDistributedHost
        where TService : class, TContract
    {
        private string _serverAddress;
        private string _serviceName;

        public WebHttpHost(string serverAddress, string serviceName)
             : base(typeof(TService), GetBasicAddress(serverAddress, serviceName))
        {
            _serverAddress = serverAddress;
            _serviceName = serviceName;

            WebHttpBinding binding = new WebHttpBinding();
            ServiceEndpoint endpoint = AddServiceEndpoint(typeof(TContract), binding, GetBasicAddress(serverAddress, serviceName));
            WebHttpBehavior httpBehavior = new WebHttpBehavior();
            endpoint.Behaviors.Add(httpBehavior);
        }

        public string ServerAddress
        {
            get { return _serverAddress; }
        }

        public string ServiceName
        {
            get { return _serviceName; }
        }

        private static Uri GetBasicAddress(string serverAddress, string serviceName)
        {
            return new Uri(string.Format("{0}/{1}/", serverAddress, serviceName));
        }
    }
}
