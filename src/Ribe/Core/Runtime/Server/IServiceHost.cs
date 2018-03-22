using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Runtime.Server
{
    public interface IServiceHost
    {
        int Port { get; }

        Task StartAsync();

        Task ShutdownAsync();
    }
}
