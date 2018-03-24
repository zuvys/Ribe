using Ribe.Core;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Runtime.Client.Invoker
{
    public interface IServiceInvoker
    {
        Task<object> InvokeAsync(Type valueType, object[] paramterValues, RequestHeader options);
    }
}
