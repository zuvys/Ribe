using System.Threading.Tasks;

using Ribe.Messaging;
using Ribe.Rpc.Transport;

using DotNetty.Transport.Channels;

namespace Ribe.Rpc.DotNetty.Core.Runtime.Server
{
    public class DotNettyServerMessageSender : IMessageSender
    {
        private IChannelHandlerContext _context;

        public DotNettyServerMessageSender(IChannelHandlerContext context)
        {
            _context = context;
        }

        public Task SendAsync(Message message)
        {
            return _context.WriteAndFlushAsync(message);
        }
    }
}
