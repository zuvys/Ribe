using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Client;
using Ribe.Codecs;
using Ribe.Core.Service.Address;
using Ribe.DotNetty.Adapter;
using Ribe.DotNetty.Messaging;
using Ribe.Json.Codecs;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Client
{
    public class DotNettyClientFactory : IRpcClientFacotry, IDisposable
    {
        private Bootstrap _bootstrap;

        private IEventLoopGroup _group;

        private ConcurrentDictionary<string, TaskCompletionSource<Message>> _map;

        private ISerializerProvider _serializerProvider;

        public DotNettyClientFactory(ISerializerProvider serializerProvider)
        {
            _serializerProvider = serializerProvider;

            _map = new ConcurrentDictionary<string, TaskCompletionSource<Message>>();
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
                    pip.AddLast(new ChannelDecoderAdapter(new DecoderProvider(new[] { new JsonDecoder() })));
                    pip.AddLast(new ChannelEncoderAdapter(new EncoderProvider(new[] { new JsonEncoder() })));
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

                        if (_map.TryRemove(id, out var tcs))
                        {
                            tcs.SetResult(message);
                        }
                    }));
                }));
        }

        public IRpcClient CreateClient(ServiceAddress address)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(address.Ip), address.Port);
            var channel = _bootstrap.ConnectAsync(endPoint).Result;
            var sender = new DotNettyClientMessageSender(channel, _serializerProvider);

            return new RpcClient(sender, _map);
        }

        public void Dispose()
        {
            _group.ShutdownGracefullyAsync();
        }
    }
}
