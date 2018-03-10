using DotNetty.Transport.Channels;
using Ribe.Messaging;
using Ribe.Transport;
using System;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Messaging
{
    public class DotNettyMessageSender : IMessageSender, IDisposable
    {
        private IChannel _channel;

        private bool _keepConnectionAlive;

        public DotNettyMessageSender(IChannel channel, bool keepConnectionAlive = true)
        {
            _channel = channel;
            _keepConnectionAlive = keepConnectionAlive;
        }

        public Task SendAsync(IMessage message)
        {
            return _channel.WriteAndFlushAsync(message);
        }

        public void Dispose()
        {
            if (!_keepConnectionAlive)
            {
                _channel.DisconnectAsync().WithNoWaiting();
            }
        }
    }
}
