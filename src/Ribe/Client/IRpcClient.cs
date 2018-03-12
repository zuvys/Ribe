using Ribe.Messaging;

using System;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public interface IRpcClient : IDisposable
    {
        Task<long> SendRequestAsync(RemoteCallMessage message);

        Task<Message> GetReponseAsync(long requestId);
    }
}
