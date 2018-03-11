using System.Threading.Tasks;

namespace Ribe.Messaging
{
    public interface IClientMessageSender : IMessageSender
    {
        Task SendAsync(RemoteCallMessage invokeMessage);
    }
}
