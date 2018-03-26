using System.Threading.Tasks;

namespace Ribe.Rpc.Runtime.Client.Invoker
{
    public interface IServiceInvoker
    {
        Task<object> InvokeAsync(Invocation req);
    }
}
