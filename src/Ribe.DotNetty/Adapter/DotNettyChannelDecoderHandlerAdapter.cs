using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ribe.Codecs;

using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.DotNetty.Adapter
{
    public class DotNettyChannelDecoderHandlerAdapter : ByteToMessageDecoder
    {
        private IDecoderManager _decoderProvider;

        public DotNettyChannelDecoderHandlerAdapter(IDecoderManager decoderProvider)
        {
            _decoderProvider = decoderProvider;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if (input.Capacity == 0)
            {
                return;
            }

            var content = new byte[input.Capacity - input.Array[input.ArrayOffset] - 1];
            var contentType = Encoding.UTF8.GetString(
                input.Array,
                input.ArrayOffset + 1,
                input.Array[input.ArrayOffset]);

            Array.Copy(
                input.Array,
                input.ArrayOffset + input.Array[input.ArrayOffset] + 1,
                content,
                0,
                input.Capacity - 1 - input.Array[input.ArrayOffset]);

            var decoder = _decoderProvider.GetDecoder(contentType);
            if (decoder == null)
            {
                throw new NullReferenceException(nameof(decoder));
            }

            context.FireChannelRead(decoder.Decode(content));
        }
    }
}
