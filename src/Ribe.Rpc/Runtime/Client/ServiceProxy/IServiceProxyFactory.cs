namespace Ribe.Rpc.Runtime.Client.ServiceProxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>();
    }
}
