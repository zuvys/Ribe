using Ribe.Messaging;
using Ribe.Rpc.Transport;
using Ribe.Serialize;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public class RpcClient : IRpcClient
    {
        static long Seed;

        private IMessageSender _sender;

        private ISerializerProvider _serializerProvider;

        private ConcurrentDictionary<long, TaskCompletionSource<Message>> _map;

        static RpcClient()
        {
            Seed = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public RpcClient(
            IMessageSender sender,
            ISerializerProvider serializerProvider,
            ConcurrentDictionary<long, TaskCompletionSource<Message>> map)
        {
            _serializerProvider = serializerProvider ?? throw new NullReferenceException(nameof(serializerProvider));
            _sender = sender ?? throw new NullReferenceException(nameof(sender));
            _map = map ?? throw new NullReferenceException(nameof(map));
        }

        public async Task<Message> SendAsync(RequestMessage request)
        {
            var id = Interlocked.Add(ref Seed, 1);
            var serializer = _serializerProvider.GetSerializer(request.Headers[Constants.ContentType]);

            if (serializer == null)
            {
                throw new NotSupportedException($"the request content-type:{request.Headers[Constants.ContentType]} is not supported!");
            }
            else
            {
                request.Headers[Constants.RequestId] = id.ToString();
            }

            var task = _map.GetOrAdd(id, (k) => new TaskCompletionSource<Message>()).Task;

            await _sender.SendAsync(new Message(
                request.Headers, 
                serializer.SerializeObject(request.ParamterValues))
            );

            return await task;
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
