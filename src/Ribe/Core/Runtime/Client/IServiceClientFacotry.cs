using Ribe.Core.Service.Address;

namespace Ribe.Rpc.Core.Runtime.Client
{
    public interface IServiceClientFacotry
    {
        IServiceClient Create(ServiceAddress address);
    }
}
