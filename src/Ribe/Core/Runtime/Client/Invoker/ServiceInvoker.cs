﻿using Ribe.Core;
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

        private IMessageFormatterProvider _convertorProvider;

        public ServiceAddress ServiceAddress { get; internal set; }

        public ServiceInvoker(IServiceClientFacotry clientFactory, IMessageFormatterProvider convetorProvider)
        {
            _clientFactory = clientFactory;
            _convertorProvider = convetorProvider;
        }

        public async Task<object> InvokeAsync(Type valueType, object[] paramterValues, RequestHeader header)
        {
            var context = new RequestContext(header, paramterValues, valueType);
            var client = _clientFactory.Create(ServiceAddress);

            var message = await client.SendRequestAsync(context);

            if (context.IsVoidRequest)
            {
                return context.IsAsyncRequest ? Task.CompletedTask : null;
            }

            if (message == null)
            {
                throw new NullReferenceException(nameof(message));
            }

            var convertor = _convertorProvider.GetFormatter(message);
            if (convertor == null)
            {
                throw new NotSupportedException("not supported!");
            }

            var data = convertor.FormatResponse(message, context.ResponseValueType);
            if (data == null)
            {
                throw new RpcException("return value is null");
            }

            if (!string.IsNullOrEmpty(data.Error))
            {
                throw new RpcException(data.Error);
            }

            return context.IsAsyncRequest ? Task.FromResult(data.Data) : data.Data;
        }
    }
}
