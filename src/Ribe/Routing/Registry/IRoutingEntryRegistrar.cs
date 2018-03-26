namespace Ribe.Rpc.Routing.Registry
{
    public interface IRoutingEntryRegistrar
    {
        void Register(RoutingEntry entry);

        void UnRegister(RoutingEntry entry);
    }
}
