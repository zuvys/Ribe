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
        private IServiceClientFacotry _clientFactory;

        private IMessageFormatterManager _formatterProvider;

        public ServiceAddress Address { get; internal set; }

        public ServiceInvoker(
            IServiceClientFacotry clientFactory, 
            IMessageFormatterManager formatterProvider
        )
        {
            _clientFactory = clientFactory;
            _formatterProvider = formatterProvider;
        }

        public async Task<object> InvokeAsync(RequestContext req)
        {
            var client = _clientFactory.Create(Address);
            var message = await client.SendRequestAsync(req);

            if (req.IsVoidRequest)
            {
                return req.IsAsyncRequest ? Task.CompletedTask : null;
            }

            if (message == null)
            {
                throw new NullReferenceException(nameof(message));
            }

            var convertor = _formatterProvider.GetFormatter(message);
            if (convertor == null)
            {
                throw new NotSupportedException("not supported!");
            }

            var data = convertor.FormatResponse(message, req.ResponseValueType);
            if (data == null)
            {
                throw new RpcException("return value is null");
            }

            if (!string.IsNullOrEmpty(data.Error))
            {
                throw new RpcException(data.Error);
            }

            return req.IsAsyncRequest ? Task.FromResult(data.Data) : data.Data;
        }
    }
}
