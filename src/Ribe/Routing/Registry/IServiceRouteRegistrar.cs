namespace Ribe.Rpc.Routing.Registry
{
    public interface IServiceRouteRegistrar
    {
        void Register(ServiceRoutingEntry entry);

        void UnRegister(ServiceRoutingEntry entry);
    }
}
