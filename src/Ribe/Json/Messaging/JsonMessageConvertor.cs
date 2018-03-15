using Ribe.Core;
using Ribe.Json.Serialize;
using Ribe.Messaging;
using System;
using System.Collections.Generic;

namespace Ribe.Json.Messaging
{
    public class JsonMessageConvertor : IMessageConvertor
    {
        private JsonSerializer _serializer;

        public JsonMessageConvertor()
        {
            _serializer = new JsonSerializer();
        }

        public bool CanConvert(Message message)
        {
            if (message == null || message.Headers == null)
            {
                return false;
            }

            return message.Headers.GetValueOrDefault(Constants.ContentType).ToLower() == "json";
        }

        public Result ConvertToResponse(Message message, Type valueType)
        {
            if (message == null)
            {
                return null;
            }

            var entry = _serializer.DeserializeObject<Result>(message.Content);
            if (entry.Data != null)
            {
                entry.Data = _serializer.DeserializeObject(entry.Data.ToString(), valueType);
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
                var jsonValues = _serializer.DeserializeObject<object[]>(message.Content);

                for (var i = 0; i < parameterTypes.Length; i++)
                {
                    parameterValues[i] = _serializer.DeserializeObject(jsonValues[i].ToString(), parameterTypes[i]);
                }

                return parameterValues;
            });
        }
    }
}
