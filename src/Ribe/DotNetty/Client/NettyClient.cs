using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Ribe.Client;
using Ribe.Core.Service.Address;
using Ribe.DotNetty.Adapter;
using Ribe.Json.Codecs;
using Ribe.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Client
{
    public class NettyClient : IClient
    {
        private IChannel _channel;

        private Bootstrap _bootstrap;

        private ConcurrentDictionary<string, TaskCompletionSource<IMessage>> _requestIdResultMap;

        public NettyClient()
        {
            _requestIdResultMap = new ConcurrentDictionary<string, TaskCompletionSource<IMessage>>();

            Initialize();
        }

        public virtual void Initialize()
        {
            _bootstrap = new Bootstrap();
            _bootstrap
                .Group(new MultithreadEventLoopGroup())
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
                    pip.AddLast(new ChannelClientHandlerAdapter((message) => HandleReceivedMessage(message)));
                }));
        }

        public Task CloseAsync()
        {
            if (_channel != null)
            {
                return _channel.CloseAsync();
            }

            return Task.CompletedTask;
        }

        public async Task ConnectAsync(ServiceAddress address)
        {
            if (_channel == null)
            {
                _channel = await _bootstrap.ConnectAsync(
                    new IPEndPoint(IPAddress.Parse(address.Ip),
                    address.Port));
            }
        }

        public void HandleReceivedMessage(IMessage message)
        {
            if (message == null)
            {
                throw new Exception("null");
            }

            var id = message.Headers.GetValueOrDefault(Constants.RequestId);
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception($"Result Headers not constains {Constants.RequestId} key");
            }

            if (_requestIdResultMap.TryGetValue(id, out var tcs))
            {
                tcs.SetResult(message);
            }
        }

        public Task<IMessage> InvokeAsync(IMessage message)
        {
            var id = message.Headers.GetValueOrDefault(Constants.RequestId);
            var tcs = new TaskCompletionSource<IMessage>();

            _requestIdResultMap.TryAdd(id, tcs);

            if (_channel != null)
            {
                _channel.WriteAndFlushAsync(message);
            }

            return tcs.Task;
        }

        public void Dispose()
        {
            CloseAsync().Wait();
        }
    }
}
