using Ribe.Messaging;
using System.Threading.Tasks;

namespace Ribe.Rpc.Transport
{
    public interface IMessageSender
    {
        Task SendAsync(Message message);

        Task SendAsync(RequestMessage request);

        Task SendAsync(ResponseMessage response);
    }
}
