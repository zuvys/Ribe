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

        public Result ConvertToResult(Message message, Type valueType)
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

        public ServiceContext ConvertToServiceContext(Message message, Type[] paramterTypes)
        {
            return new ServiceContext(message, null);
        }
    }
}
