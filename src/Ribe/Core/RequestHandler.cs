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
            IServiceEntryProvider serviceEntryProvider
        )
        {
            _serviceExecutor = serviceExecutor;
            _serviceEntryProvider = serviceEntryProvider;
        }

        public async Task HandleRequestAsync(Request request, Func<long, Response, Task> onCompleted)
        {
            var entry = _serviceEntryProvider.GetServiceEntry(request);
            if (entry == null)
            {
                throw new Exception();
            }

            var method = entry.MethodMap.GetValueOrDefault(request.Headers[Constants.ServiceMethodKey]);
            if (method == null)
            {
                throw new Exception();
            }

            var paramterTypes = method.Parameters.Select(i => i.ParameterType).ToArray();
            var parameterValues = request.GetRequestParamterValues(paramterTypes);

            var context = new ExecutionContext()
            {
                ServiceType = entry.Implemention,
                ServiceMethod = method,
                ParamterValues = parameterValues
            };

            if (long.TryParse(request.Headers.GetValueOrDefault(Constants.RequestId), out long id))
            {
                try
                {
                    await onCompleted(id, Response.Ok(await _serviceExecutor.ExecuteAsync(context)));
                }
                catch (Exception e)
                {
                    await onCompleted(id, Response.Failed(e.Message));
                }
            }
            else
            {
                throw new NotSupportedException($"the request not contains request id!");
            }
        }
    }
}
