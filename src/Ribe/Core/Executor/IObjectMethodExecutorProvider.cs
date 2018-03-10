namespace Ribe.Core.Executor
{
    public interface IObjectMethodExecutorProvider
    {
        IObjectMethodExecutor GetExecutor(ExecutionContext context);
    }
}
