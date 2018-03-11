using Ribe.Messaging;

using System;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public interface IRpcClient : IDisposable
    {
        Task<string> SendRequestAsync(RemoteCallMessage message);

        Task<Message> GetReponseAsync(string requestId);
    }
}
