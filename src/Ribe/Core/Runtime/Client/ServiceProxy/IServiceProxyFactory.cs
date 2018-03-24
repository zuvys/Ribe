using Ribe.Core;
using System;

namespace Ribe.Rpc.Core.Runtime.Client.ServiceProxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>(Func<RequestHeader> optionBuilder);
    }
}
