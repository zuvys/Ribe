using System.Threading.Tasks;

namespace Ribe.Server
{
    public interface IRpcServer
    {
        Task StartAsync(int port);

        Task StopAsync();
    }
}
