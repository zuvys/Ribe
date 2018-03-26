using Ribe.Rpc.Logging;
using System;
using System.Collections.Generic;

namespace Ribe.Core.Service.Internals
{
    public class ServicePathFactory : IServicePathFacotry
    {
        private ILogger _logger;

        public ServicePathFactory(ILogger logger)
        {
            _logger = logger;
        }

        public string CreatePath(Type serviceType, ServiceAttribute attr)
        {
            return CreatePath(serviceType.Namespace + "." + serviceType.Name, attr);
        }

        public string CreatePath(Type serviceType, Dictionary<string, string> description)
        {
            return CreatePath(serviceType.Namespace + "." + serviceType.Name, description);
        }

        public string CreatePath(string serviceName, Dictionary<string, string> description)
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

            return CreatePath(serviceName, attr);
        }

        private string CreatePath(string serviceName, ServiceAttribute attr)
        {
            var servicePath = string.Format(@"{0}/{1}/{2}", serviceName, attr.Group, attr.Version);

            if (_logger.IsEnabled(LogLevel.Info))
                _logger.Info($"generated service path :{servicePath}");

            return servicePath.Replace("//", "/");
        }
    }
}
