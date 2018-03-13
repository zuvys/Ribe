using Ribe.Messaging;
using System.Threading.Tasks;

namespace Ribe.Messaging
{
    public interface IMessageSender
    {
        Task SendAsync(Message message);
    }
}
