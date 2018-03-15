using Ribe.Core.Executor;
using Ribe.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ribe.Core
{
    public class RequestHandler : IRequestHandler
    {
        private IServiceExecutor _serviceExecutor;

        private IServiceEntryProvider _serviceEntryProvider;

        public RequestHandler(
            IServiceExecutor serviceExecutor,
            IServiceEntryProvider serviceEntryProvider)
        {
            _serviceExecutor = serviceExecutor;
            _serviceEntryProvider = serviceEntryProvider;
        }

        public Task<Result> HandleRequestAsync(Request request)
        {
            var serviceEntry = _serviceEntryProvider.GetServiceEntry(request);
            if (serviceEntry == null)
            {
                throw new Exception();
            }

            var method = serviceEntry.MethodMap.GetValueOrDefault(request.Headers[Constants.ServiceMethodKey]);
            if (method == null)
            {
                throw new Exception();
            }

            var paramterTypes = method.Parameters.Select(i => i.ParameterType).ToArray();
            var parameterValues = request.GetRequestParamterValues(paramterTypes);

            var context = new ExecutionContext()
            {
                ServiceType = serviceEntry.Implemention,
                ServiceMethod = method,
                ParamterValues = parameterValues
            };

            try
            {
                return Task.FromResult(Result.Ok(_serviceExecutor.ExecuteAsync(context)));
            }
            catch (Exception e)
            {
                return Task.FromResult(Result.Failed(e.Message));
            }
        }
    }
}
