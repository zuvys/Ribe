using System;

namespace Ribe.Client.ServiceProxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>(Func<RpcServiceProxyOption> optionBuilder);
    }
}
