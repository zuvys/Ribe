using System.Threading.Tasks;

namespace Ribe.Messaging
{
    public interface IRequestMessageSender : IMessageSender
    {
        Task SendAsync(RequestMessage requestMessage);
    }
}
