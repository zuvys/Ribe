using Ribe.Core.Service.Address;
using System.Threading.Tasks;

namespace Ribe.Rpc.Server
{
    public abstract class RpcServer
    {
        public string Ip => Address.Ip;

        public int Port => Address.Port;

        public ServiceAddress Address { get; }

        protected RpcServer(string ip, int port)
            : this(new ServiceAddress() { Ip = ip, Port = port })
        {

        }

        protected RpcServer(ServiceAddress address)
        {
            Address = address;
        }

        public abstract Task StartAsync();

        public abstract Task StopAsync();
    }
}
