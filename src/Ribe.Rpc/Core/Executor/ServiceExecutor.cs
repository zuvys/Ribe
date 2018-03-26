using Ribe.Rpc.Core.Service;
using Ribe.Rpc.Logging;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Executor
{
    public class ServiceExecutor : IServiceExecutor
    {
        private ILogger _logger;

        private IObjectMethodExecutorProvider _methodExecutorProvider;

        private IServiceActivator _serviceActivator;

        public ServiceExecutor(
            IServiceActivator serviceActivator,
            IObjectMethodExecutorProvider methodExecutorProvider,
            ILogger logger)
        {
            _serviceActivator = serviceActivator;
            _methodExecutorProvider = methodExecutorProvider;
            _logger = logger;
        }

        public async Task<object> ExecuteAsync(ExecutionContext context)
        {
            var service = _serviceActivator.Create(context.ServiceType);
            var methodExecutor = _methodExecutorProvider.GetExecutor(context);

            try
            {
                return await methodExecutor.ExecuteAsync(service, context.ParamterValues);
            }
            finally
            {
                _serviceActivator.Release(service);
            }
        }
    }
}
