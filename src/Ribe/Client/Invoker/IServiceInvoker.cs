using System;

namespace Ribe.Client.Invoker
{
    public interface IServiceInvoker
    {
        object InvokeAsync(Type valueType, object[] paramterValues, RpcServiceProxyOption options);
    }
}
