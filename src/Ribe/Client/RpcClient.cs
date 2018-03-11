using Ribe.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public class RpcClient : IRpcClient
    {
        private IClientMessageSender _sender;

        private ConcurrentDictionary<string, TaskCompletionSource<Message>> _requestMap;

        public RpcClient(
            IClientMessageSender sender,
            ConcurrentDictionary<string, TaskCompletionSource<Message>> requestMap)
        {
            if (_sender == null)
            {
                throw new NullReferenceException(nameof(_sender));
            }

            if (_requestMap == null)
            {
                throw new NullReferenceException(nameof(_requestMap));
            }

            _sender = sender;
            _requestMap = requestMap;
        }

        public async Task<string> SendRequestAsync(RemoteCallMessage message)
        {
            var id = message.Headers.GetValueOrDefault(Constants.RequestId);
            if (!string.IsNullOrEmpty(id))
            {
                await _sender.SendAsync(message);
                return id;
            }

            throw new RpcException("request id is empty!");
        }

        public async Task<Message> GetReponseAsync(string id)
        {
            return await _requestMap.GetOrAdd(id.ToString(), (k) => new TaskCompletionSource<Message>()).Task;
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
