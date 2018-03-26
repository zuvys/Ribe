using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Executor
{
    public interface IServiceExecutor
    {
        Task<object> ExecuteAsync(ExecutionContext context);
    }

    //IRequestHandler
}
