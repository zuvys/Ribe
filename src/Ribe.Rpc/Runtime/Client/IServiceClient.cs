using Ribe.Rpc.Messaging;
using System.Threading.Tasks;

namespace Ribe.Rpc.Runtime.Client
{
    public interface IServiceClient
    {
        Task<Message> SendRequestAsync(Invocation context);
    }
}
