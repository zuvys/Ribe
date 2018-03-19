using System;
using System.Threading.Tasks;

using Ribe.Messaging;
using Ribe.Rpc.Transport;

using DotNetty.Transport.Channels;

namespace Ribe.DotNetty
{
    public class DotNettyClientMessageSender : IMessageSender, IDisposable
    {
        private IChannel _channel;

        private bool _closeChannel;

        public DotNettyClientMessageSender(IChannel channel, bool closeChannel = true)
        {
            _channel = channel;
            _closeChannel = closeChannel;
        }

        public Task SendAsync(Message message)
        {
            return _channel.WriteAndFlushAsync(message);
        }

        public void Dispose()
        {
            if (_closeChannel)
            {
                _channel.CloseAsync().Wait();
            }
        }
    }
}
