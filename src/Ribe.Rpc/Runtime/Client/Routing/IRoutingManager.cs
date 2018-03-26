namespace Ribe.Rpc.Runtime.Client.Routing
{
    public interface IRoutingManager : IRouter
    {
        void AddRouter(IRouter router);

        void RemoveRouter(IRouter router);
    }
}
