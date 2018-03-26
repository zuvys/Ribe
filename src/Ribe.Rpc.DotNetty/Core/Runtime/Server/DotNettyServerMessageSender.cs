using DotNetty.Transport.Channels;
using Ribe.Rpc.Messaging;
using Ribe.Rpc.Transport;
using System.Threading.Tasks;

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
            return _context.WriteAsync(message);
        }
    }
}
