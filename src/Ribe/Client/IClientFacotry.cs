using Ribe.Core.Service.Address;

namespace Ribe.Client
{
    public interface IClientFacotry
    {
        IClient CreateClient(ServiceAddress address);
    }
}
