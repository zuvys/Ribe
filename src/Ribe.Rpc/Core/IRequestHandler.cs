using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core
{
    public interface IRequestHandler
    {
        Task HandleRequestAsync(Request request, Func<long, Response, Task> reqCallBack);
    }
}
