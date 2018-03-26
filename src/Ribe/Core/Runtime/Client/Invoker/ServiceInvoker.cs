using Ribe.Core;
using Ribe.Core.Service.Address;
using Ribe.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Runtime.Client.Invoker
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

        public async Task<object> InvokeAsync(RequestContext req)
        {
            var message = await _client.SendRequestAsync(req);

            if (req.IsVoidRequest)
            {
                return req.IsAsyncRequest ? Task.CompletedTask : null;
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

            var data = formatter.FormatResponse(message, req.ResponseValueType);
            if (data == null)
            {
                return req.IsAsyncRequest ? Task.FromResult<object>(null) : null;
            }

            if (!string.IsNullOrEmpty(data.Error))
            {
                throw new RpcException(data.Error);
            }

            return req.IsAsyncRequest ? Task.FromResult(data.Data) : data.Data;
        }
    }
}
