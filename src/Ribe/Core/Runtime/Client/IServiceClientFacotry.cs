using Ribe.Core.Service.Address;
using Ribe.Rpc.Core.Runtime.Client;

namespace Ribe.Rpc.Core.Runtime.Client
{
    public interface IServiceClientFacotry
    {
        IServiceClient Create(ServiceAddress address);
    }
}
