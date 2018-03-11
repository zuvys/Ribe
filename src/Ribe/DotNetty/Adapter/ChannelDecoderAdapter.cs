using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ribe.Codecs;

using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelDecoderAdapter : ByteToMessageDecoder
    {
        private IDecoderProvider _decoderProvider;

        public ChannelDecoderAdapter(IDecoderProvider decoderProvider)
        {
            _decoderProvider = decoderProvider;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var content = new byte[input.Capacity - input.Array[input.ArrayOffset]];
            var contentType = Encoding.UTF8.GetString(
                input.Array,
                input.ArrayOffset,
                input.Array[input.ArrayOffset]);

            Array.Copy(
                input.Array,
                input.ArrayOffset + input.Array[input.ArrayOffset],
                content,
                0,
                input.Capacity);

            var decoder = _decoderProvider.GetDecoder(contentType);
            if (decoder == null)
            {
                throw new Exception();
            }

            context.FireChannelRead(decoder.Decode(content));
        }
    }
}
