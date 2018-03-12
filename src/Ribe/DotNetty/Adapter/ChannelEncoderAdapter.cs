using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ribe.Codecs;
using Ribe.Messaging;

using System;
using System.Collections.Generic;
using System.Text;

namespace Ribe.DotNetty.Adapter
{
    public class ChannelEncoderAdapter : MessageToMessageEncoder<Message>
    {
        private IEncoderProvider _encoderProvider;

        public ChannelEncoderAdapter(IEncoderProvider encoderProvider)
        {
            _encoderProvider = encoderProvider;
        }

        protected override void Encode(IChannelHandlerContext context, Message input, List<object> output)
        {
            if (input != null)
            {
                var contentType = input.Headers.GetValueOrDefault(Constants.ContentType);
                if (string.IsNullOrEmpty(contentType))
                {
                    throw new NotSupportedException($"the ContentType with empty is not supported!");
                }

                var encoder = _encoderProvider.GetEncoder(contentType);
                if (encoder == null)
                {
                    throw new NotSupportedException($"the ContentType with {contentType} is not supported!");
                }

                var bytes = encoder.Encode(input);
                var codec = Encoding.UTF8.GetBytes(contentType);

                var buffer = context.Allocator.Buffer(bytes.Length + codec.Length);

                buffer.WriteByte(codec.Length);
                buffer.WriteBytes(codec);
                buffer.WriteBytes(bytes);

                output.Add(buffer);
            }
        }
    }
}
