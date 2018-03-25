using org.apache.zookeeper;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Zookeeper
{
    public class ZkNodeWatcher : Watcher
    {
        public Action<string, Event.EventType> OnNodeChanged { get; }

        public Action OnDisconnected { get; }

        public ZkNodeWatcher(Action onDisconnected, Action<string, Event.EventType> onNodeChanged)
        {
            OnNodeChanged = onNodeChanged;
            OnDisconnected = onDisconnected;
        }

        public override Task process(WatchedEvent @event)
        {
            if (@event.getState() == Event.KeeperState.Disconnected)
            {
                OnDisconnected();
                return Task.CompletedTask;
            }

            return Task.Run(() => OnNodeChanged(@event.getPath(), @event.get_Type()));
        }
    }
}
