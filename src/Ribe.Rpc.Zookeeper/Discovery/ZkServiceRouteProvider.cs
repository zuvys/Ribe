using org.apache.zookeeper;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Routing;
using Ribe.Rpc.Routing.Discovery;
using Ribe.Serialize;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static org.apache.zookeeper.Watcher;

namespace Ribe.Rpc.Zookeeper.Discovery
{
    public class ZkServiceRouteProvider : IServiceRouteProvider
    {
        private ILogger _logger;

        private ZooKeeper _zooKeeper;

        private ZkNodeWatcher _zkNodeWatcher;

        private ZkConfiguration _zkConfiguration;

        private ISerializerProvider _serializerProvider;

        private ConcurrentDictionary<string, List<ServiceRoutingEntry>> _caches;

        public ZkServiceRouteProvider(ZkConfiguration zkConfiguration, ISerializerProvider serializerProvider, ILogger logger)
        {
            _logger = logger;
            _zkConfiguration = zkConfiguration;
            _serializerProvider = serializerProvider;
            _caches = new ConcurrentDictionary<string, List<ServiceRoutingEntry>>();
            _zkNodeWatcher = new ZkNodeWatcher(() => CreateZkeeper(), (path, type) => OnZkNodeChanged(path, type));
            CreateZkeeper();
        }

        public List<ServiceRoutingEntry> GetRoutes(string serivceName)
        {
            return _caches.GetOrAdd(_zkConfiguration.RootPath + "/" + serivceName, (path) =>
            {
                if (_zooKeeper.existsAsync(path, _zkNodeWatcher).Result == null)
                {
                    return new List<ServiceRoutingEntry>();
                }

                var routes = new List<ServiceRoutingEntry>();

                FindChildrenRoutes(path, routes);

                return routes;
            });
        }

        private void FindChildrenRoutes(string path, List<ServiceRoutingEntry> routes)
        {
            if (_zooKeeper.existsAsync(path, _zkNodeWatcher).Result == null)
            {
                return;
            }

            var result = _zooKeeper.getChildrenAsync(path, _zkNodeWatcher).Result;
            if (result.Children.Any())
            {
                foreach (var child in result.Children)
                {
                    FindChildrenRoutes(path + "/" + child, routes);
                }

                return;
            }

            var serializer = _serializerProvider.GetSerializer(Constants.Json);
            if (serializer == null)
            {
                throw new NullReferenceException(nameof(serializer));
            }

            var data = _zooKeeper.getDataAsync(path, _zkNodeWatcher).Result.Data;
            if (data != null)
            {
                routes.Add(serializer.DeserializeObject<ServiceRoutingEntry>(data));
            }

            return;
        }

        private void CreateZkeeper()
        {
            if (_zooKeeper != null)
            {
                _zooKeeper.closeAsync().Wait();
            }

            _zooKeeper = new ZooKeeper(_zkConfiguration.Address, _zkConfiguration.SessionTimeout, _zkNodeWatcher);
        }

        private void OnZkNodeChanged(string path, Event.EventType eventType)
        {
            var routes = new List<ServiceRoutingEntry>();

            switch (eventType)
            {
                case Event.EventType.NodeCreated:
                case Event.EventType.NodeDeleted:
                case Event.EventType.NodeDataChanged:
                case Event.EventType.NodeChildrenChanged:
                    if (path.Length < _zkConfiguration.RootPath.Length)
                    {
                        FindChildrenRoutes(path, routes);
                    }
                    else
                    {
                        FindChildrenRoutes(
                            path.Substring(0,
                            path.IndexOf("/", _zkConfiguration.RootPath.Length + 1)),
                            routes);
                    }
                    break;
            }

            if (routes.Count != 0)
            {
                foreach (var item in routes.GroupBy(i => i.ServiceName))
                {
                    var entries = item.ToList();

                    _caches.AddOrUpdate(
                        _zkConfiguration.RootPath + "/" + item.FirstOrDefault().ServiceName,
                        entries,
                        (k, v) => entries);
                }
            }
        }
    }
}
