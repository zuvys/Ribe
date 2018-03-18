using Ribe.Core;
using Ribe.Messaging;
using Ribe.Rpc.Json.Serialize;
using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Json.Messaging
{
    public class JsonMessageConvertor : IMessageConvertor
    {
        public bool CanConvert(Message message)
        {
            if (message == null || message.Headers == null)
            {
                return false;
            }

            return message.Headers.GetValueOrDefault(Constants.ContentType).ToLower() == "json";
        }

        public Response ConvertToResponse(Message message, Type valueType)
        {
            if (message == null)
            {
                return null;
            }

            var entry = JsonSerializer.Default.DeserializeObject<Response>(message.Content);
            if (entry.Data != null)
            {
                entry.Data = JsonSerializer.Default.DeserializeObject(entry.Data.ToString(), valueType);
            }

            return entry;
        }

        public Request ConvertToRequest(Message message)
        {
            return new Request(message.Content, message.Headers, (parameterTypes) =>
            {
                if (parameterTypes == null || parameterTypes.Length == 0)
                {
                    return null;
                }

                var parameterValues = new object[parameterTypes.Length];
                var jsonValues = JsonSerializer.Default.DeserializeObject<object[]>(message.Content);

                for (var i = 0; i < parameterTypes.Length; i++)
                {
                    parameterValues[i] = JsonSerializer.Default.DeserializeObject(jsonValues[i].ToString(), parameterTypes[i]);
                }

                return parameterValues;
            });
        }
    }
}
