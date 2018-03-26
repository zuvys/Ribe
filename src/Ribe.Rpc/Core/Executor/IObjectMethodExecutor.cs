using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Executor
{
    public interface IObjectMethodExecutor
    {
        Task<object> ExecuteAsync(object instance, object[] paramterValues);
    }
}
