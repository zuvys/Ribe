using System;

namespace Ribe.Client.Proxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>(Func<ServiceProxyOption> optionBuilder);
    }
}
