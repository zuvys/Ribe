using System.Threading.Tasks;

namespace Ribe.Core.Executor
{
    public interface IObjectMethodExecutor
    {
        object Execute(object instance, object[] paramterValues);

        Task<object> ExecuteAsync(object instance, object[] paramterValues);
    }
}
