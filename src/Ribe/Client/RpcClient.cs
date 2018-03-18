using Ribe.Messaging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public class RpcClient : IRpcClient
    {
        private static long IdSeed;

        private IRequestMessageSender _sender;

        private ConcurrentDictionary<long, TaskCompletionSource<Message>> _map;

        static RpcClient()
        {
            IdSeed = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public RpcClient(
            IRequestMessageSender sender,
            ConcurrentDictionary<long, TaskCompletionSource<Message>> map)
        {
            _sender = sender ?? throw new NullReferenceException(nameof(sender));
            _map = map ?? throw new NullReferenceException(nameof(map));
        }

        public async Task<Message> SendAsync(RequestMessage request)
        {
            var id = Interlocked.Add(ref IdSeed, 1);

            request.Headers[Constants.RequestId] = id.ToString();
            _map.GetOrAdd(id, (k) => new TaskCompletionSource<Message>());

            await _sender.SendAsync(request);

            return await _map.GetOrAdd(id, (k) => new TaskCompletionSource<Message>()).Task;
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
