namespace Ribe.Rpc.Core.Runtime.Server
{
    public interface IServiceHostFactory
    {
        IServiceHost Create(int port);
    }
}
