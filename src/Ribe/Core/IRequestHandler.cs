using System.Threading.Tasks;

namespace Ribe.Core
{
    public interface IRequestHandler
    {
        Task<Response> HandleRequestAsync(Request request);
    }
}
