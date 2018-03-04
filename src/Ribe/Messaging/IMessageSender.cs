using Ribe.Messaging;
using System.Threading.Tasks;

namespace Ribe.Transport
{
    public interface IMessageSender
    {
        Task SendAsync(IMessage message);
    }
}
