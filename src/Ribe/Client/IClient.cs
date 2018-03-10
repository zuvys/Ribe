using Ribe.Messaging;

using System;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public interface IClient : IDisposable
    {
        Task<IMessage> SendMessageAsync(IMessage message);
    }
}
