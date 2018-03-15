using System.Threading.Tasks;

namespace Ribe.Core.Executor
{
    public interface IServiceExecutor
    {
        Task<object> ExecuteAsync(ExecutionContext context);
    }

    //IRequestHandler
}
