using Ribe.Rpc.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Runtime.Client
{
    public interface IServiceClient:IDisposable
    {
        Task<Message> SendRequestAsync(Invocation context);
    }
}
