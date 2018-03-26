using Ribe.Rpc.Core.Service.Address;

namespace Ribe.Rpc.Runtime.Client
{
    public interface IServiceClientFacotry
    {
        IServiceClient Create(ServiceAddress address);
    }
}
