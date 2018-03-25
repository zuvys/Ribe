using Ribe.Rpc.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ribe.Core.Service.Internals
{
    public class ServiceFactory : IServiceFactory
    {
        private IServiceMethodNameMapFactory _methodNameMapFactory;

        private IServiceNameFacotry _serviceNameFacotry;

        private ILogger _logger;

        public ServiceFactory(
            IServiceNameFacotry serviceNameFacotry,
            IServiceMethodNameMapFactory methodNameMapFactory,
            ILogger logger)
        {
            _serviceNameFacotry = serviceNameFacotry;
            _methodNameMapFactory = methodNameMapFactory;
            _logger = logger;
        }

        public List<ServiceEntry> CreateServices(Type serviceType)
        {
            if (!serviceType.IsClass || serviceType.IsAbstract)
            {
                throw new NotSupportedException($"the service of type {serviceType.FullName} is not supported");
            }

            var attr = GetServiceAttribute(serviceType);
            var serviceEntries = new List<ServiceEntry>();

            foreach (var def in serviceType.GetInterfaces())
            {
                var map = _methodNameMapFactory.CreateMap(def, serviceType);
                if (map.Count == 0)
                {
                    continue;
                }

                var serviceName = _serviceNameFacotry.CreateName(def, attr);
                var serviceEntry = new ServiceEntry()
                {
                    Attribute = attr,
                    ServiceType = serviceType,
                    Methods = map,
                    ServiceName = _serviceNameFacotry.CreateName(def, attr)
                };

                serviceEntries.Add(serviceEntry);
            }

            return serviceEntries;
        }

        private static ServiceAttribute GetServiceAttribute(Type serviceType)
        {
            var attr = serviceType.GetCustomAttribute<ServiceAttribute>();
            if (attr == null)
            {
                throw new NotImplementedException($"the type of {serviceType.FullName} have not a RpcServiceAttribute!");
            }

            return attr;
        }
    }
}
