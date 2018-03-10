using System.Threading.Tasks;

namespace Ribe.Core
{
    public interface IServiceInvoker
    {
        Task InvokeAsync();
    }

    public interface IServerInvokerFacotry
    {
        IServiceInvoker CreateInvoker(ServiceContext context);
    }

    public class DefaultServiceInvokerFacotry : IServerInvokerFacotry
    {
        public IServiceInvoker CreateInvoker(ServiceContext context)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DefaultServiceInvoker : IServiceInvoker
    {
        public DefaultServiceInvoker(ServiceContext context)
        {

        }

        public Task InvokeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
