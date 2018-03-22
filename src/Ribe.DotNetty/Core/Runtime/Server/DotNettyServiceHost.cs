using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Rpc.Core.Runtime.Server;
using System;
using System.Threading.Tasks;

namespace Ribe.Rpc.DotNetty.Core.Runtime.Server
{
    public class DotNettyServiceHost : IServiceHost, IDisposable
    {
        private object _syncObject = new object();

        private bool _started;

        private IChannelHandler _handler;

        private MultithreadEventLoopGroup _bossGroup;

        private MultithreadEventLoopGroup _workerGroup;

        public int Port { get; }

        public DotNettyServiceHost(int port, IChannelHandler handler)
        {
            _handler = handler;
            Port = port;
        }

        public Task StartAsync()
        {
            lock (_syncObject)
            {
                if (!_started)
                {
                    new ServerBootstrap()
                        .Group(_bossGroup, _workerGroup)
                        .Channel<TcpServerSocketChannel>()
                        .Option(ChannelOption.SoBacklog, 100)
                        .Handler(new LoggingHandler("SRV-LSTN"))
                        .ChildHandler(_handler)
                        .BindAsync(Port)
                        .Wait();
                }

                return Task.CompletedTask;
            }
        }

        public Task ShutdownAsync()
        {
            lock (_syncObject)
            {
                if (_started)
                {
                    if (_bossGroup != null)
                    {
                        _bossGroup.ShutdownGracefullyAsync().Wait();
                    }

                    if (_workerGroup != null)
                    {
                        _workerGroup.ShutdownGracefullyAsync().Wait();
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            ShutdownAsync().Wait();
        }
    }
}
