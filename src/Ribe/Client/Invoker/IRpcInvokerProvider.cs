namespace Ribe.Client.Invoker
{
    public interface IRpcInvokerProvider
    {
        IRpcInvoker GetInvoker(/*参数后续考虑*/);
    }
}
