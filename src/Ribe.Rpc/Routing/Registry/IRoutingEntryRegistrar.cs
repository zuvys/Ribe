namespace Ribe.Rpc.Runtime.Client.Routing.Registry
{
    public interface IRoutingEntryRegistrar
    {
        void Register(RoutingEntry entry);

        void UnRegister(RoutingEntry entry);
    }
}
