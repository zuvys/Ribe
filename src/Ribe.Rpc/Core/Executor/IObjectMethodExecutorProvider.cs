namespace Ribe.Rpc.Core.Executor
{
    public interface IObjectMethodExecutorProvider
    {
        IObjectMethodExecutor GetExecutor(ExecutionContext context);
    }
}
