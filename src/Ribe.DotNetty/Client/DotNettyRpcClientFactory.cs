using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Client;
using Ribe.Codecs;
using Ribe.Core.Service.Address;
using Ribe.DotNetty.Adapter;
using Ribe.Messaging;
using Ribe.Rpc.Transport;
using Ribe.Serialize;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Client
{
    public class DotNettyRpcClientFactory : IRpcClientFacotry, IDisposable
    {
        private Bootstrap _bootstrap;

        private IEventLoopGroup _group;

        private ConcurrentDictionary<long, TaskCompletionSource<Message>> _map;

        private ISerializerProvider _serializerProvider;

        public DotNettyRpcClientFactory(
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
                    channel.Pipeline.AddLast(new LoggingHandler());
                    channel.Pipeline.AddLast(new LengthFieldPrepender(4));
                    channel.Pipeline.AddLast(new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                    channel.Pipeline.AddLast(new DotNettyChannelDecoderHandlerAdapter(decoderProvider));
                    channel.Pipeline.AddLast(new DotNettyChannelEncoderHandlerAdapter(encoderProvider));
                    channel.Pipeline.AddLast(new DotNettyChannelClientHandlerAdapter((message) =>
                    {
                        var requestId = message.Headers.GetValueOrDefault(Constants.RequestId);
                        if (requestId == null)
                        {
                            throw new Exception($"Response Headers not constains {Constants.RequestId} key");
                        }

                        if (long.TryParse(requestId, out var id))
                        {
                            if (_map.TryRemove(id, out var tcs))
                            {
                                tcs.SetResult(message);
                                return;
                            }
                        }
                    }));
                }));
        }

        public RpcClient Create(ServiceAddress address)
        {
            try
            {
                return new RpcClient(
                    new DotNettyRpcClientMessageSender(_bootstrap.ConnectAsync(address.ToEndPoint()).Result),
                    _serializerProvider,
                    _map);
            }
            catch (Exception e)
            {
                throw new ConnectException($"connect to server faield!ip:{address.Ip},port:{address.Port}", e);
            }
        }

        public void Dispose()
        {
            _group.ShutdownGracefullyAsync();
        }
    }
}
