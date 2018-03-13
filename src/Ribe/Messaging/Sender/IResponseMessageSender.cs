using System.Threading.Tasks;

namespace Ribe.Messaging
{
    public interface IResponseMessageSender : IMessageSender
    {
        Task SendAsync(ResponseMessage responseMessage);
    }
}
