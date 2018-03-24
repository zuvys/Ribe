using Ribe.Rpc.Logging;
using System;
using System.Collections.Generic;

namespace Ribe.Core.Service.Internals
{
    public class ServiceMethodNameMapFacotry : IServiceMethodNameMapFactory
    {
        private IServiceMethodNameFactory _serviceMthodKeyFactory;

        private ILogger _logger;

        public ServiceMethodNameMapFacotry(IServiceMethodNameFactory serviceMthodKeyFactory, ILogger logger)
        {
            _serviceMthodKeyFactory = serviceMthodKeyFactory;
            _logger = logger;
        }

        public Dictionary<string, ServiceMethod> CreateMethodMap(Type @interface, Type servieType)
        {
            var map = servieType.GetInterfaceMap(@interface);
            var nameMap = new Dictionary<string, ServiceMethod>();

            if (map.TargetMethods.Length == 0)
            {
                return nameMap;
            }

            foreach (var impl in map.TargetMethods)
            {
                var serviceMethodKey = _serviceMthodKeyFactory.CreateName(impl);

                var serviceMethod = new ServiceMethod()
                {
                    Method = impl,
                    Parameters = impl.GetParameters()
                };

                nameMap[serviceMethodKey] = serviceMethod;
            }

            return nameMap;
        }
    }
}
