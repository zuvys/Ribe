using Microsoft.Extensions.Logging;
using Ribe.Core.Service;
using System.Threading.Tasks;

namespace Ribe.Core.Executor
{
    public class ServiceExecutor : IServiceExecutor
    {
        private ILogger<ServiceExecutor> _logger;

        private IObjectMethodExecutorProvider _objectMethodExecutorProvider;

        private IServiceActivator _serviceActivator;

        public ServiceExecutor(
            IServiceActivator serviceActivator,
            IObjectMethodExecutorProvider objectMethodExecutorProvider,
            ILogger<ServiceExecutor> logger)
        {
            _serviceActivator = serviceActivator;
            _objectMethodExecutorProvider = objectMethodExecutorProvider;
            _logger = logger;
        }

        public Task<object> ExecuteAsync(ExecutionContext context)
        {
            var methodExecutor = _objectMethodExecutorProvider.GetExecutor(context);
            var service = _serviceActivator.Create(context.ServiceType);

            return methodExecutor.ExecuteAsync(service, context.ParamterValues);
        }
    }
}
