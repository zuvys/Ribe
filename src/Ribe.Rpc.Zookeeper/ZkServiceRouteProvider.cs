using org.apache.zookeeper;
using Ribe.Rpc.Logging;
using Ribe.Rpc.Runtime.Client.Routing;
using Ribe.Rpc.Serialize;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static org.apache.zookeeper.Watcher;

namespace Ribe.Rpc.Zookeeper.Discovery
{
    public class ZkServiceRouteProvider : IRoutingEntryProvider
    {
        private ILogger _logger;

        private ZooKeeper _zooKeeper;

        private ZkNodeWatcher _zkNodeWatcher;

        private ZkConfiguration _zkConfiguration;

        private ISerializerManager _serializerProvider;

        private ConcurrentDictionary<string, List<RoutingEntry>> _caches;

        public ZkServiceRouteProvider(ZkConfiguration zkConfiguration, ISerializerManager serializerProvider, ILogger logger)
        {
            _logger = logger;
            _zkConfiguration = zkConfiguration;
            _serializerProvider = serializerProvider;
            _caches = new ConcurrentDictionary<string, List<RoutingEntry>>();
            _zkNodeWatcher = new ZkNodeWatcher(() => CreateZkeeper(), (path, type) => OnZkNodeChanged(path, type));

            if (string.IsNullOrEmpty(zkConfiguration.RootPath))
            {
                zkConfiguration.RootPath = string.Empty;
            }

            if (!zkConfiguration.RootPath.StartsWith("/"))
            {
                zkConfiguration.RootPath = "/" + zkConfiguration.RootPath;
            }

            if (zkConfiguration.RootPath.EndsWith("/"))
            {
                zkConfiguration.RootPath = zkConfiguration.RootPath.Remove(zkConfiguration.RootPath.Length - 1);
            }

            CreateZkeeper();
        }

        public List<RoutingEntry> GetRoutes(string serivceName)
        {
            return _caches.GetOrAdd(_zkConfiguration.RootPath + "/" + serivceName, (path) =>
            {
                if (_zooKeeper.existsAsync(path, _zkNodeWatcher).Result == null)
                {
                    return new List<RoutingEntry>();
                }

                var routes = new List<RoutingEntry>();

                FindChildrenRoutes(path, routes);

                return routes;
            });
        }

        private void FindChildrenRoutes(string path, List<RoutingEntry> routes)
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
                routes.Add(serializer.DeserializeObject<RoutingEntry>(data));
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
            var routes = new List<RoutingEntry>();

            switch (eventType)
            {
                case Event.EventType.NodeDeleted:
                    OnZkNodeDeleted(path);
                    break;
                case Event.EventType.NodeCreated:
                case Event.EventType.NodeDataChanged:
                case Event.EventType.NodeChildrenChanged:
                    FindChildrenRoutes(path, routes);
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

        private void OnZkNodeDeleted(string path)
        {
            //Clear Cache
            foreach (var item in _caches.Keys.ToList())
            {
                if (path.StartsWith(item))
                {
                    _caches.TryRemove(item, out var _);
                }
            }
        }
    }
}
