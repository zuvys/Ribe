using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ribe.Codecs;
using System;
using System.Collections.Generic;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelDecoderAdapter : ByteToMessageDecoder
    {
        private IDecoder _decoder;

        public ChannelDecoderAdapter(IDecoder decoder)
        {
            _decoder = decoder;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var content = new byte[input.Capacity];

            Array.Copy(input.Array, input.ArrayOffset, content, 0, input.Capacity);

            context.FireChannelRead(_decoder.Decode(content));
        }
    }
}
