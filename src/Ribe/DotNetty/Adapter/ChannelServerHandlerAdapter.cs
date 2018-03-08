using DotNetty.Transport.Channels;
using Ribe.Core;
using Ribe.Core.Executor;
using Ribe.Core.Service;
using Ribe.DotNetty.Messaging;
using Ribe.Json;
using Ribe.Json.Messaging;
using Ribe.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelServerHandlerAdapter : ChannelHandlerAdapter
    {
        private IServiceEntryProvider _serviceEntryProvider;

        private IServceExecutor _servceExecutor;

        public ChannelServerHandlerAdapter(
            IServiceEntryProvider serviceEntryProvider,
            IServceExecutor servceExecutor)
        {
            _servceExecutor = servceExecutor;
            _serviceEntryProvider = serviceEntryProvider;
        }

        public async override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            var message = (IMessage)msg;
            if (message != null)
            {
                var ctx = message.GetInvokeContext();
                var entry = _serviceEntryProvider.GetServiceEntry(ctx);
                var method = entry.GetServiceMethod(ctx);
                var paramterTypes = method.Parameters.Select(i => i.ParameterType).ToArray();
                var parameterValues = ctx.ParamterValuesConvertor(paramterTypes, message.Body);
                
                var executionContext = new ServiceExecutionContext()
                {
                    ParamterValues = parameterValues,
                    ServiceMethod = method,
                    ServiceType = entry.Implemention,
                    Headers = message.Headers
                };

                var value = await _servceExecutor.ExecuteAsync(executionContext);
                var serializer = message.Serializer;
                var result = new Result
                {
                    Data = value,
                    Error = string.Empty,
                    RequestId = ctx.RequestId,
                    Status = Status.Success
                };

                await new NettyMessageSender(context).SendAsync(new JsonMessage()
                {
                    Body = serializer.SerializeObject(result),
                    Headers = new Dictionary<string, string>()
                    {
                        [Constants.RequestId] = message.Headers[Constants.RequestId]
                    }
                });
            }
        }
    }
}
