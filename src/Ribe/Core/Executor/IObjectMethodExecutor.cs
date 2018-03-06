using System.Threading.Tasks;

namespace Ribe.Core.Executor
{
    public interface IObjectMethodExecutor
    {
        Task<object> ExecuteAsync(object instance, object[] paramterValues);
    }
}
