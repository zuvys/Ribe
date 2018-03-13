using Ribe.Messaging;

using System;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public interface IRpcClient : IDisposable
    {
        Task<long> SendRequestAsync(RequestMessage message);

        Task<Message> GetReponseAsync(long requestId);
    }
}
