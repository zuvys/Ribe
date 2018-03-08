using System;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker
{
    public interface IServiceInvoker
    {
        Task<object> InvokeAsync(Type valueType, object[] paramterValues, RpcServiceProxyOption options);
    }
}
