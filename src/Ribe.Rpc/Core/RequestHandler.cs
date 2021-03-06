﻿using Ribe.Rpc.Core.Executor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core
{
    public class RequestHandler : IRequestHandler
    {
        private IServiceExecutor _executor;

        private Service.IServiceEntryProvider _entryProvider;

        public RequestHandler(IServiceExecutor executor, Service.IServiceEntryProvider entryProvider)
        {
            _executor = executor;
            _entryProvider = entryProvider;
        }

        public async Task HandleRequestAsync(Request req, Func<long, Response, Task> reqCallBack)
        {
            var entry = _entryProvider.GetEntry(req);
            if (entry == null)
            {
                await reqCallBack(req.RequestId, Response.Create(null, "service entry not found!", Status.EntryNotFound));
                return;
            }

            var method = entry.Methods.GetValueOrDefault(req.Header[Constants.MethodName]);
            if (method == null)
            {
                await reqCallBack(req.RequestId, Response.Create(null, "service method not found!", Status.MethodNotFound));
                return;
            }

            var paramterTypes = method.Parameters.Select(i => i.ParameterType).ToArray();
            var parameterValues = req.GetRequestParamterValues(paramterTypes);

            var context = new ExecutionContext()
            {
                ServiceType = entry.ServiceType,
                ServiceMethod = method,
                ParamterValues = parameterValues
            };

            try
            {
                await reqCallBack(req.RequestId, Response.Ok(await _executor.ExecuteAsync(context)));
            }
            catch (Exception e)
            {
                await reqCallBack(req.RequestId, Response.Failed(e.Message));
            }
        }
    }
}
