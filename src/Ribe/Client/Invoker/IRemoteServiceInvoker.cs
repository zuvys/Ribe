using System;
using System.Threading.Tasks;

namespace Ribe.Client.Invoker
{
    public interface IRemoteServiceInvoker
    {
        Task<object> InvokeAsync(Type valueType, object[] paramterValues, ServiceProxyOption options);
    }
}
