using Ribe.Messaging;

using System;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public interface IRpcClient : IDisposable
    {
        Task<IMessage> InvokeAsync(IMessage message);
    }
}
