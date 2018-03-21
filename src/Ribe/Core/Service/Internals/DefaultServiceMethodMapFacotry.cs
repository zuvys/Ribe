using Ribe.Rpc.Logging;
using System;
using System.Collections.Generic;

namespace Ribe.Core.Service.Internals
{
    public class DefaultServiceMethodMapFacotry : IServiceMethodMapFacotry
    {
        private IServiceMethodKeyFactory _serviceMthodKeyFactory;

        private ILogger _logger;

        public DefaultServiceMethodMapFacotry(IServiceMethodKeyFactory serviceMthodKeyFactory, ILogger logger)
        {
            _serviceMthodKeyFactory = serviceMthodKeyFactory;
            _logger = logger;
        }

        public Dictionary<string, ServiceMethod> CreateMethodMap(Type @interface, Type servieType)
        {
            var interfaceMap = servieType.GetInterfaceMap(@interface);
            var serviceMethodMap = new Dictionary<string, ServiceMethod>();

            if (interfaceMap.TargetMethods.Length == 0)
            {
                return serviceMethodMap;
            }

            foreach (var impl in interfaceMap.TargetMethods)
            {
                var serviceMethodKey = _serviceMthodKeyFactory.CreateMethodKey(impl);

                var serviceMethod = new ServiceMethod()
                {
                    Method = impl,
                    Parameters = impl.GetParameters()
                };

                serviceMethodMap[serviceMethodKey] = serviceMethod;
            }

            return serviceMethodMap;
        }
    }
}
