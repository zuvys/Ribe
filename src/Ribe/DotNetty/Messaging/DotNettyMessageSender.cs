using DotNetty.Transport.Channels;
using Ribe.Messaging;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Messaging
{
    public class DotNettyMessageSender : IMessageSender
    {
        protected IChannel Channel { get; }

        public DotNettyMessageSender(IChannel channel)
        {
            Channel = channel;
        }

        public Task SendAsync(Message message)
        {
            return Channel.WriteAndFlushAsync(message);
        }
    }
}
