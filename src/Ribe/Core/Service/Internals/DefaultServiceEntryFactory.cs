using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ribe.Core.Service.Internals
{
    public class DefaultServiceEntryFactory : IServiceEntryFactory
    {
        private IServiceMethodMapFacotry _serviceMethodMapFactory;

        private IServiceEntryPathFacotry _servicePathFacotry;

        private ILogger _logger;

        public DefaultServiceEntryFactory(
            IServiceEntryPathFacotry servicePathFacotry,
            IServiceMethodMapFacotry serviceMethodMapFactory,
            ILogger logger)
        {
            _servicePathFacotry = servicePathFacotry;
            _serviceMethodMapFactory = serviceMethodMapFactory;
            _logger = logger;
        }

        public List<ServiceEntry> CreateServices(Type serviceType)
        {
            if (!serviceType.IsClass || serviceType.IsAbstract)
            {
                throw new NotSupportedException($"the service of type {serviceType.FullName} is not supported");
            }

            var attr = GetServiceAttribute(serviceType);
            var services = new List<ServiceEntry>();

            foreach (var def in serviceType.GetInterfaces())
            {
                var service = new ServiceEntry()
                {
                    Attribute = attr,
                    Interface = def,
                    Implemention = serviceType,
                    MethodMap = _serviceMethodMapFactory.CreateMethodMap(def, serviceType),
                    ServicePath = _servicePathFacotry.CreatePath(def, attr)
                };

                if (service.MethodMap.Count == 0)
                {
                    continue;
                }

                services.Add(service);
            }

            return services;
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
