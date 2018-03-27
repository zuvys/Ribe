using Ribe.Rpc.Core.Service.Address;

namespace Ribe.Rpc.Runtime.Client.ServiceProxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>();

        TService CreateProxy<TService>(ServiceAddress addr);
    }
}
