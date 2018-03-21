using Ribe.Rpc.Logging;
using System;

namespace Ribe.Core.Service.Internals
{
    public class DefaultServiceEntryPathFactory : IServiceEntryPathFacotry
    {
        private ILogger _logger;

        public DefaultServiceEntryPathFactory(ILogger logger)
        {
            _logger = logger;
        }

        public string CreatePath(Type serviceType, ServiceAttribute rpc)
        {
            var serviceName = serviceType.Namespace + "." + serviceType.Name;
            var servicepath = string.Format(@"/{0}/{1}/{2}/", rpc.Group, serviceName, rpc.Version);

            if (_logger.IsEnabled(LogLevel.Info))
            {
                _logger.Info($"created service path :{servicepath}");
            }

            return servicepath;
        }
    }
}
