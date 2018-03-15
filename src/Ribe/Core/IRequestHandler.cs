using System.Threading.Tasks;

namespace Ribe.Core
{
    public interface IRequestHandler
    {
        Task<Result> HandleRequestAsync(Request request);
    }
}
