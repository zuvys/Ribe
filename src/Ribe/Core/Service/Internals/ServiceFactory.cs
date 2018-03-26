using Ribe.Rpc.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ribe.Core.Service.Internals
{
    public class ServiceFactory : IServiceFactory
    {
        private IServiceMethodNameMapFactory _methodNameMapFactory;

        private IServicePathFacotry _serviceNameFacotry;

        private ILogger _logger;

        public ServiceFactory(
            IServicePathFacotry serviceNameFacotry,
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

                var serviceName = _serviceNameFacotry.CreatePath(def, attr);
                var serviceEntry = new ServiceEntry()
                {
                    Attribute = attr,
                    ServiceType = serviceType,
                    Methods = map,
                    ServicePath = _serviceNameFacotry.CreatePath(def, attr),
                    ServiceName = def.Namespace + "." + def.Name
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
