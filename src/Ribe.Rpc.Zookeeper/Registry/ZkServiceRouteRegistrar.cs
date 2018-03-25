﻿using Ribe.Rpc.Logging;
using Ribe.Rpc.Routing;
using Ribe.Rpc.Routing.Registry;
using Ribe.Serialize;
using System;
using org.apache.zookeeper;
using static org.apache.zookeeper.ZooDefs;
using System.Linq;

namespace Ribe.Rpc.Zookeeper.Registry
{
    public class ZkServiceRouteRegistrar : IServiceRouteRegistrar, IDisposable
    {
        private ILogger _logger;

        private ZooKeeper _zooKeeper;

        private ZkConfiguration _zkConfiguration;

        private ISerializerProvider _serializerProvider;

        public ZkServiceRouteRegistrar(ZkConfiguration zkConfiguration, ISerializerProvider serializerProvider, ILogger logger)
        {
            _logger = logger;
            _zkConfiguration = zkConfiguration;
            _serializerProvider = serializerProvider;
            CreateZkeeper();
        }

        public void Register(ServiceRoutingEntry entry)
        {
            var path = _zkConfiguration.RootPath + "/" + entry.ServiceName + "/host" + entry.Address;
            var serializer = _serializerProvider.GetSerializer(Constants.Json);
            if (serializer == null)
            {
                throw new NullReferenceException(nameof(serializer));
            }

            if (_zooKeeper.existsAsync(path, false).Result == null)
            {
                if (_logger.IsEnabled(LogLevel.Info))
                    _logger.Info($"节点不存在,准备创建:{path}");

                CreateChildrenNodes(path);
            }

            if (_zooKeeper.existsAsync(path, false).Result != null)
            {
                _zooKeeper.setDataAsync(path, serializer.SerializeObject(entry), -1).Wait();
                return;
            }

            if (_logger.IsEnabled(LogLevel.Error))
                _logger.Error($"节点注册失败{path}");
        }

        public void UnRegister(ServiceRoutingEntry entry)
        {
            var path = _zkConfiguration.RootPath + "/" + entry.ServiceName;

            if (_zooKeeper.existsAsync(path, false).Result == null)
            {
                if (_logger.IsEnabled(LogLevel.Info))
                    _logger.Info("节点不存在,已取消注册");
                return;
            }

            _zooKeeper.deleteAsync(path, -1).Wait();
        }

        protected virtual void CreateChildrenNodes(string path)
        {
            if (_logger.IsEnabled(LogLevel.Info))
                _logger.Info("节点不存在:" + path + "准备创建!");

            var nodes = path.Split('/').Where(item => !string.IsNullOrEmpty(item)).ToArray();
            var nodePath = string.Empty;

            for (var i = 0; i < nodes.Length; i++)
            {
                nodePath += "/" + nodes[i];

                if (_zooKeeper.existsAsync(nodePath, false).Result == null)
                {
                    _zooKeeper.createAsync(
                        nodePath,
                        null,
                        Ids.OPEN_ACL_UNSAFE,
                        i == nodes[i].Length - 1 ? CreateMode.EPHEMERAL_SEQUENTIAL : CreateMode.PERSISTENT
                    ).Wait();

                    if (_logger.IsEnabled(LogLevel.Info))
                        _logger.Info($"节点不存在,准备创建:{nodePath}");

                    continue;
                }

                if (_logger.IsEnabled(LogLevel.Info))
                    _logger.Info($"节点已存在:{nodePath}");
            }
        }

        private void CreateZkeeper()
        {
            if (_zooKeeper != null)
            {
                _zooKeeper.closeAsync().Wait();
            }

            var kcWatcher = new KeepConnectionWatcher(() =>
            {
                CreateZkeeper();
            });

            _zooKeeper = new ZooKeeper(_zkConfiguration.Address, _zkConfiguration.SessionTimeout, kcWatcher);
        }

        public void Dispose()
        {
            if (_zooKeeper != null)
            {
                _zooKeeper.closeAsync().Wait();
            }
        }
    }
}
