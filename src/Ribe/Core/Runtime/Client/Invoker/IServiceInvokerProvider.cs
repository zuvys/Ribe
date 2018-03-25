namespace Ribe.Rpc.Core.Runtime.Client.Invoker
{
    public interface IServiceInvokerProvider
    {
        IServiceInvoker GetInvoker(RequestContext req);
    }
}
