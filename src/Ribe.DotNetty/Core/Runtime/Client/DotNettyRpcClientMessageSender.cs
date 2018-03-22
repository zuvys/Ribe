using DotNetty.Transport.Channels;

using Ribe.Messaging;
using Ribe.Rpc.Transport;

using System;
using System.Threading.Tasks;

namespace Ribe.DotNetty
{
    public class DotNettyRpcClientMessageSender : IMessageSender, IDisposable
    {
        private IChannel _channel;

        private bool _closeChannel;

        public DotNettyRpcClientMessageSender(IChannel channel, bool closeChannel = true)
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
