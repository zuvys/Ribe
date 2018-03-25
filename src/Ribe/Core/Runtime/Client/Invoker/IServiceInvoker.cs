using Ribe.Core.Service.Address;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Runtime.Client.Invoker
{
    public interface IServiceInvoker
    {
        ServiceAddress Address { get; }

        Task<object> InvokeAsync(RequestContext req);
    }
}
