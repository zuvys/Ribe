namespace Ribe.Rpc.Runtime.Server
{
    public interface IServiceHostFactory
    {
        IServiceHost Create(int port);
    }
}
