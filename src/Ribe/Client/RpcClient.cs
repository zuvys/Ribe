using Ribe.Core;
using Ribe.Messaging;
using Ribe.Rpc.Transport;
using Ribe.Serialize;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public class RpcClient : IDisposable
    {
        protected static long Seed;

        protected IMessageSender Sender { get; }

        protected ISerializerProvider SerializerProvider { get; }

        protected ConcurrentDictionary<long, TaskCompletionSource<Message>> Map { get; }

        static RpcClient()
        {
            Seed = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public RpcClient(
            IMessageSender sender,
            ISerializerProvider serializerProvider,
            ConcurrentDictionary<long, TaskCompletionSource<Message>> map)
        {
            Map = map ?? throw new NullReferenceException(nameof(map));
            Sender = sender ?? throw new NullReferenceException(nameof(sender));
            SerializerProvider = serializerProvider ?? throw new NullReferenceException(nameof(serializerProvider));
        }

        public virtual async Task<Message> SendRequestAsync(RequestContext context)
        {
            var serializer = SerializerProvider.GetSerializer(context.RequestHeaders[Constants.ContentType]);
            if (serializer == null)
            {
                throw new NotSupportedException($"the request content-type:{context.RequestHeaders[Constants.ContentType]} is not supported!");
            }

            if (!long.TryParse(context.RequestHeaders[Constants.RequestId], out var id))
            {
                id = Interlocked.Add(ref Seed, 1);
                context.RequestHeaders[Constants.RequestId] = id.ToString();
            }

            SendMessage(new Message(
                context.RequestHeaders,
                serializer.SerializeObject(context.RequestParamterValues)
            ));

            if (context.IsVoidRequest)
            {
                return null;
            }

            return await Map.GetOrAdd(id, (k) => new TaskCompletionSource<Message>()).Task;
        }

        private async void SendMessage(Message message)
        {
            await Sender.SendAsync(message);
        }

        public virtual void Dispose()
        {
            if (Sender is IDisposable dispose)
            {
                dispose.Dispose();
            }
        }
    }
}
