using Ribe.Core;
using Ribe.Messaging;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Runtime.Client
{
    public interface IServiceClient
    {
        Task<Message> SendRequestAsync(RequestContext context);
    }
}
