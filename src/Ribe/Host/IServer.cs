using System.Threading.Tasks;

namespace Ribe.Host
{
    public interface IServer
    {
        Task StartAsync();

        Task StopAsync();
    }
}
