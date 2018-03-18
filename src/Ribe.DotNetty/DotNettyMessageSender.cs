using DotNetty.Transport.Channels;
using Ribe.Messaging;
using Ribe.Rpc.Transport;
using Ribe.Serialize;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.DotNetty
{
    public class DotNettyMessageSenderAdapter : MessageSenderAdapter, IDisposable
    {
        private IChannel _channel;

        private bool _disposing;

        public DotNettyMessageSenderAdapter(
            IChannel channel,
            bool disposing,
            ISerializerProvider serializerProvider
        ) : base(serializerProvider)
        {
            _channel = channel;
            _disposing = disposing;
        }

        public override Task SendAsync(Message message)
        {
            return _channel.WriteAndFlushAsync(message);
        }

        public void Dispose()
        {
            if (_disposing)
            {
                _channel.CloseAsync();
            }
        }
    }
}
