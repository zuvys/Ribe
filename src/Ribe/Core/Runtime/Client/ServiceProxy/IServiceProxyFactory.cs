using Ribe.Core;
using System;

namespace Ribe.Client.ServiceProxy
{
    public interface IServiceProxyFactory
    {
        TService CreateProxy<TService>(Func<RequestHeader> optionBuilder);
    }
}
