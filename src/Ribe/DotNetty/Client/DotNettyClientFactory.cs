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

        private ConcurrentDictionary<long, TaskCompletionSource<Message>> _map;

        private ISerializerProvider _serializerProvider;

        public DotNettyClientFactory(
            ISerializerProvider serializerProvider,
            IEncoderProvider encoderProvider,
            IDecoderProvider decoderProvider
            )
        {
            _serializerProvider = serializerProvider;

            _map = new ConcurrentDictionary<long, TaskCompletionSource<Message>>();
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
                    pip.AddLast(new ChannelDecoderAdapter(decoderProvider));
                    pip.AddLast(new ChannelEncoderAdapter(encoderProvider));
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

                        var requestId = message.Headers.GetValueOrDefault(Constants.RequestId);
                        if (requestId == null)
                        {
                            throw new Exception($"Result Headers not constains {Constants.RequestId} key");
                        }

                        if (long.TryParse(requestId, out long id))
                        {
                            if (_map.TryRemove(id, out var tcs))
                            {
                                tcs.SetResult(message);
                                return;
                            }

                            throw new Exception($"the request with id {id} was not found!");
                        }

                        throw new NotSupportedException("request id must type with long");
                    }));
                }));
        }

        public IRpcClient CreateClient(ServiceAddress address)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(address.Ip), address.Port);
            var channel = _bootstrap.ConnectAsync(endPoint).Result;
            var sender = new DotNettyRequestMessageSender(channel, _serializerProvider);

            return new RpcClient(sender, _map);
        }

        public void Dispose()
        {
            _group.ShutdownGracefullyAsync();
        }
    }
}
