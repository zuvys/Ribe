using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Client;
using Ribe.Core.Service.Address;
using Ribe.DotNetty.Adapter;
using Ribe.Json.Codecs;
using System;
using System.Collections.Generic;
using System.Net;

namespace Ribe.DotNetty.Client
{
    public class RpcClientFactory : IRpcClientFacotry, IDisposable
    {
        private Bootstrap _bootstrap;

        private IEventLoopGroup _group;

        private InvocationCompletionSource _completionSource;

        public RpcClientFactory()
        {
            _completionSource = new InvocationCompletionSource();
            _group = new MultithreadEventLoopGroup();
            _bootstrap = new Bootstrap()
                .Group(_group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    var pip = channel.Pipeline;
                    pip.AddLast(new LoggingHandler());
                    pip.AddLast(new LengthFieldPrepender(4));
                    pip.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                    pip.AddLast(new ChannelDecoderAdapter(new JsonDecoder()));
                    pip.AddLast(new ChannelEncoderAdapter(new JsonEncoder()));
                    pip.AddLast(new ChannelClientHandlerAdapter((message) =>
                    {
                        if (message == null)
                        {
                            throw new Exception("message is null!");
                        }

                        if (message.Headers == null)
                        {
                            throw new NullReferenceException("message headers is null!");
                        }

                        var id = message.Headers.GetValueOrDefault(Constants.RequestId);
                        if (id == null)
                        {
                            throw new Exception($"Result Headers not constains {Constants.RequestId} key");
                        }

                        _completionSource.SetResult(id, message);
                    }));
                }));
        }

        public IRpcClient CreateClient(ServiceAddress address)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(address.Ip), address.Port);
            var channel = _bootstrap.ConnectAsync(endPoint).Result;

            return new RpcClient(
                () => channel.CloseAsync(),
                (m) => channel.WriteAndFlushAsync(m),
                (id) => _completionSource.GetResult(id)
            );
        }

        public void Dispose()
        {
            _group.ShutdownGracefullyAsync();
        }
    }
}
