namespace Ribe.Rpc.Core.Runtime.Client.Invoker
{
    public interface IServiceInvokerProvider
    {
        IServiceInvoker GetInvoker(/*参数后续考虑*/);
    }
}
