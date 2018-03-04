using DotNetty.Transport.Channels;
using Ribe.Messaging;
using Ribe.Transport;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Messaging
{
    public class NettyMessageSender : IMessageSender
    {
        private IChannelHandlerContext _context;

        public NettyMessageSender(IChannelHandlerContext ctx)
        {
            _context = ctx;
        }

        public async Task SendAsync(IMessage message)
        {
            await _context.WriteAndFlushAsync(message);
        }
    }
}
