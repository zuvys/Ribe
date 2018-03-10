using Ribe.Messaging;
using Ribe.Transport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Ribe.Client
{
    public class Client : IClient
    {
        private IMessageSender _sender;

        private ConcurrentDictionary<string, TaskCompletionSource<IMessage>> _requestMap;

        public Client(
            IMessageSender sender,
            ConcurrentDictionary<string, TaskCompletionSource<IMessage>> requestMap)
        {
            _sender = sender;
            _requestMap = requestMap;
        }

        public async Task<IMessage> SendMessageAsync(IMessage message)
        {
            var id = message.Headers.GetValueOrDefault(Constants.RequestId);
            if (id == null)
            {
                throw new RpcException("request id is empty!");
            }

            if (_sender != null)
            {
                _sender.SendAsync(message).WithNoWaiting();
            }

            return await _requestMap.GetOrAdd(id, (k) => new TaskCompletionSource<IMessage>()).Task;
        }

        public void Dispose()
        {
            if (_sender is IDisposable dispose)
            {
                dispose.Dispose();
            }
        }
    }
}
