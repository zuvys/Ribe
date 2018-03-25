using Ribe.Rpc.Logging;
using System;
using System.Collections.Generic;

namespace Ribe.Core.Service.Internals
{
    public class ServiceNameFactory : IServiceNameFacotry
    {
        private ILogger _logger;

        public ServiceNameFactory(ILogger logger)
        {
            _logger = logger;
        }

        public string CreateName(Type serviceType, ServiceAttribute rpc)
        {
            var serviceName = serviceType.Namespace + "." + serviceType.Name;
            var servicePath = string.Format(@"{0}/{1}/{2}", serviceName, rpc.Group, rpc.Version);

            if (_logger.IsEnabled(LogLevel.Info))
                _logger.Info($"generated service path :{servicePath}");

            return servicePath.Replace("//", "/");
        }

        public string CreateName(Type serviceType, Dictionary<string, string> description)
        {
            var attr = new ServiceAttribute();
            if (description.ContainsKey(Constants.Group))
            {
                attr.Group = description.GetValueOrDefault(Constants.Group);
            }

            if (description.ContainsKey(Constants.Version))
            {
                attr.Version = description.GetValueOrDefault(Constants.Version);
            }

            return CreateName(serviceType, attr);
        }
    }
}
