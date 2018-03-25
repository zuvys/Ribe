namespace Ribe.Rpc.Routing
{
    public interface IRouterManager : IRouter
    {
        void AddRouter(IRouter router);

        void RemoveRouter(IRouter router);
    }
}
