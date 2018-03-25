using org.apache.zookeeper;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.Zookeeper
{
    public class KeepConnectionWatcher : Watcher
    {
        public Action OnDisconncted { get; }

        public KeepConnectionWatcher(Action onDisconnected)
        {
            OnDisconncted = onDisconnected;
        }

        public override Task process(WatchedEvent @event)
        {
            if (@event.getState() == Event.KeeperState.Disconnected)
            {
                OnDisconncted();
            }

            return Task.CompletedTask;
        }
    }
}
