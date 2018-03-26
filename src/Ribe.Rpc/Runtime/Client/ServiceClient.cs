using Ribe.Rpc.Messaging;
using Ribe.Rpc.Serialize;
using Ribe.Rpc.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ribe.Rpc.Runtime.Client
{
    public class ServiceClient :IServiceClient, IDisposable
    {
        protected static long Seed;

        protected IMessageSender Sender { get; }

        protected ISerializerManager SerializerProvider { get; }

        protected ConcurrentDictionary<long, TaskCompletionSource<Message>> Map { get; }

        static ServiceClient()
        {
            Seed = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public ServiceClient(
            IMessageSender sender,
            ISerializerManager serializerProvider,
            ConcurrentDictionary<long, TaskCompletionSource<Message>> map)
        {
            Map = map ?? throw new NullReferenceException(nameof(map));
            Sender = sender ?? throw new NullReferenceException(nameof(sender));
            SerializerProvider = serializerProvider ?? throw new NullReferenceException(nameof(serializerProvider));
        }

        public virtual async Task<Message> SendRequestAsync(Invocation context)
        {
            var serializer = SerializerProvider.GetSerializer(context.Header[Constants.ContentType]);
            if (serializer == null)
            {
                throw new NotSupportedException($"the request content-type:{context.Header[Constants.ContentType]} is not supported!");
            }

            if (!long.TryParse(context.Header.GetValueOrDefault(Constants.RequestId), out var id))
            {
                id = Interlocked.Add(ref Seed, 1);
                context.Header[Constants.RequestId] = id.ToString();
            }

            await Sender.SendAsync(new Message(
                  context.Header,
                  serializer.SerializeObject(context.ParamterValues)));

            if (context.IsVoid)
            {
                return null;
            }

            return await Map.GetOrAdd(id, (k) => new TaskCompletionSource<Message>()).Task;
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
