using System.Threading.Tasks;

namespace Ribe.Core
{
    public interface IServiceInvoker
    {
        Task InvokeAsync();
    }

    public interface IServerInvokerFacotry
    {
        IServiceInvoker CreateInvoker(ServiceRequestContext context);
    }

    public class DefaultServiceInvokerFacotry : IServerInvokerFacotry
    {
        public IServiceInvoker CreateInvoker(ServiceRequestContext context)
        {
            return new DefaultServiceInvoker(context);
        }
    }

    public class DefaultServiceInvoker : IServiceInvoker
    {
        public ServiceRequestContext Context { get; }

        public DefaultServiceInvoker(ServiceRequestContext context)
        {
            Context = context;
        }

        public Task InvokeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
