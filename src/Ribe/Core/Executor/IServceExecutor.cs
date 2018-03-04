using System.Threading.Tasks;

namespace Ribe.Core.Executor
{
    public interface IServceExecutor
    {
        Task<object> ExecuteAsync(ServiceExecutionContext context);
    }
}
