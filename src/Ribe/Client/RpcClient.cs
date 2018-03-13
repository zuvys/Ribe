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
            if (_sender == null)
            {
                throw new NullReferenceException(nameof(_sender));
            }

            if (_map == null)
            {
                throw new NullReferenceException(nameof(map));
            }

            _sender = sender;
            _map = map;
        }

        public async Task<long> SendRequestAsync(RequestMessage message)
        {
            var id = Interlocked.Add(ref IdSeed, 1);

            message.Headers[Constants.RequestId] = id.ToString();

            await _sender.SendAsync(message);
            return id;
        }

        public async Task<Message> GetReponseAsync(long id)
        {
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
