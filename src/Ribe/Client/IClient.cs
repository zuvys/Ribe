using Ribe.Core;
using Ribe.Core.Service.Address;
using Ribe.Messaging;
using System;
using System.Threading.Tasks;

namespace Ribe.Client
{
    public interface IClient : IDisposable
    {
        Task CloseAsync();

        Task ConnectAsync(ServiceAddress address);

        Task<IMessage> InvokeAsync(IMessage message);
    }
}
