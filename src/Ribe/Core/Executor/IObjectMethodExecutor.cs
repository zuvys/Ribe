namespace Ribe.Core.Executor
{
    public interface IObjectMethodExecutor
    {
        object Execute(object service, object[] paramterValues);
    }
}
