using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Ribe.Core;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ribe.DotNetty.Messaging
{
    public class DotNettyResponseMessageSender : DotNettyMessageSender, IResponseMessageSender
    {
        private ISerializerProvider _serializerProvider;

        public DotNettyResponseMessageSender(
            IChannel channel,
            ISerializerProvider serializerProvider)
            : base(channel)
        {
            _serializerProvider = serializerProvider;
        }

        public Task SendAsync(ResponseMessage responseMessage)
        {
            //can make the json serializer(or another) as the default serializer
            //so cannot be null at here,if the serializer is null ,
            //the response message or the error message,cannot be sent to client
            var serializer = _serializerProvider.GetSerializer(responseMessage.Headers[Constants.ContentType]);

            return SendAsync(new Message(
                responseMessage.Headers,
                serializer.SerializeObject(responseMessage.Result)
                )
            );
        }
    }
}
