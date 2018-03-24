using Ribe.Rpc.Logging;
using System.Reflection;
using System.Text;

namespace Ribe.Core.Service.Internals
{
    public class ServiceMethodNameFactory : IServiceMethodNameFactory
    {
        private ILogger _logger;

        public ServiceMethodNameFactory(ILogger logger)
        {
            _logger = logger;
        }

        public string CreateName(MethodInfo method)
        {
            var sb = new StringBuilder(method.Name);

            foreach (var parameter in method.GetParameters())
            {
                sb.Append("_")
                    .Append(parameter.ParameterType.Namespace)
                    .Append(".")
                    .Append(parameter.ParameterType.Name);
            }

            var serviceMethodId = sb.ToString();

            if (_logger.IsEnabled(LogLevel.Info))
            {
                _logger.Info($"created service method id {serviceMethodId}");
            }

            return serviceMethodId;
        }
    }
}
