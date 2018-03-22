using System.Threading.Tasks;

namespace Ribe.Rpc.Server
{
    public abstract class RpcServer
    {
        public virtual int Port { get; }

        public abstract Task StartAsync();

        public abstract Task ShutdownAsync();
    }
}
