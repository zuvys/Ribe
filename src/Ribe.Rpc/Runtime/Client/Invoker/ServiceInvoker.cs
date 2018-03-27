using Ribe.Rpc.Messaging.Formatting;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Runtime.Client.Invoker
{
    /// <summary>
    /// default <see cref="IServiceInvoker"/> 
    /// </summary>
    public class ServiceInvoker : IServiceInvoker
    {
        private IServiceClient _client;

        private IMessageFormatterManager _formatterManager;

        public ServiceInvoker(IServiceClient client, IMessageFormatterManager formatterManager)
        {
            _client = client;
            _formatterManager = formatterManager;
        }

        public async Task<object> InvokeAsync(Invocation req)
        {
            using (_client)
            {
                var message = await _client.SendRequestAsync(req);

                if (req.IsVoid)
                {
                    return req.IsAsync ? Task.CompletedTask : null;
                }

                if (message == null)
                {
                    throw new NullReferenceException(nameof(message));
                }

                var formatter = _formatterManager.GetFormatter(message);
                if (formatter == null)
                {
                    throw new NotSupportedException("格式化响应消息失败!");
                }

                var data = formatter.FormatResponse(message, req.ValueType);
                if (data == null)
                {
                    return req.IsAsync ? Task.FromResult<object>(null) : null;
                }

                if (!string.IsNullOrEmpty(data.Error))
                {
                    throw new RpcException(data.Error);
                }

                return req.IsAsync ? Task.FromResult(data.Data) : data.Data;
            }
        }
    }
}
