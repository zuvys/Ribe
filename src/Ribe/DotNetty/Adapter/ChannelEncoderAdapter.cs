using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ribe.Codecs;
using Ribe.Messaging;
using System.Collections.Generic;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelEncoderAdapter : MessageToMessageEncoder<IMessage>
    {
        private IEncoder _encoder;

        public ChannelEncoderAdapter(IEncoder encoder)
        {
            _encoder = encoder;
        }

        protected override void Encode(IChannelHandlerContext context, IMessage input, List<object> output)
        {
            if (input != null)
            {
                var bytes = _encoder.Encode(input);
                var buf = context.Allocator.Buffer(bytes.Length);

                buf.WriteBytes(bytes);

                output.Add(buf);
            }
        }
    }
}
