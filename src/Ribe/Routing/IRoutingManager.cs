namespace Ribe.Rpc.Routing
{
    public interface IRoutingManager : IRouter
    {
        void AddRouter(IRouter router);

        void RemoveRouter(IRouter router);
    }
}
