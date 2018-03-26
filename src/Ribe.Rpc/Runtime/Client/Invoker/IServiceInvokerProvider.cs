namespace Ribe.Rpc.Runtime.Client.Invoker
{
    public interface IServiceInvokerProvider
    {
        IServiceInvoker GetInvoker(Invocation req);
    }
}
