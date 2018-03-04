namespace Ribe.Client.Proxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>();
    }
}
