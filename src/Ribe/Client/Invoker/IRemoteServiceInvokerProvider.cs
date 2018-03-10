namespace Ribe.Client.Invoker
{
    public interface IRemoteServiceInvokerProvider
    {
        IRemoteServiceInvoker GetInvoker(/*参数后续考虑*/);
    }
}
