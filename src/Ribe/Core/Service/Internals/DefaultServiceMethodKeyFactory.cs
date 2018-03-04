using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;

namespace Ribe.Core.Service.Internals
{
    public class DefaultServiceMethodKeyFactory : IServiceMethodKeyFactory
    {
        private ILogger _logger;

        public DefaultServiceMethodKeyFactory(ILogger logger)
        {
            _logger = logger;
        }

        public string CreateMethodKey(MethodInfo method)
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

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"created service method id {serviceMethodId}");
            }

            return serviceMethodId;
        }
    }
}
