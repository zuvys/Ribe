using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ribe.Codecs;
using Ribe.Messaging;
using System.Collections.Generic;
using System.Text;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelEncoderAdapter : MessageToMessageEncoder<Message>
    {
        private IEncoder _encoder;

        const int ConentTypeLength = 1;

        public ChannelEncoderAdapter(IEncoder encoder)
        {
            _encoder = encoder;
        }

        protected override void Encode(IChannelHandlerContext context, Message input, List<object> output)
        {
            if (input != null)
            {
                var typeBytes = Encoding.UTF8.GetBytes(input.Headers.GetValueOrDefault(Constants.ContentType));
                var offset = ConentTypeLength + typeBytes.Length;

                var bytes = _encoder.Encode(input);
                var buf = context.Allocator.Buffer(bytes.Length + offset);

                buf.WriteByte((byte)typeBytes.Length);
                buf.WriteBytes(typeBytes);
                buf.WriteBytes(bytes);

                output.Add(buf);
            }
        }
    }
}
