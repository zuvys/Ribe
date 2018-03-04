using Ribe.Core;
using Ribe.Json.Serialize;
using Ribe.Messaging;
using Ribe.Serialize;
using System;
using System.Collections.Generic;

namespace Ribe.Json.Messaging
{
    public class JsonMessage : IMessage
    {
        public byte[] Body { get; set; }

        public ISerializer Serializer => new JsonSerializer();

        public Dictionary<string, string> Headers { get; set; }

        protected object[] ConvertParamterValues(Type[] paramterTypes, byte[] bytes)
        {
            if (paramterTypes == null || paramterTypes.Length == 0)
            {
                return null;
            }

            var serializer = (JsonSerializer)Serializer;
            var parameterValues = new object[paramterTypes.Length];
            var jsonValues = Serializer.DeserializeObject<object[]>(bytes);

            for (var i = 0; i < paramterTypes.Length; i++)
            {
                parameterValues[i] = serializer.DeserializeObject(jsonValues[i].ToString(), paramterTypes[i]);
            }

            return parameterValues;
        }

        public ServiceExecutionResult GetResult(Type dataType)
        {
            if (Body == null || Body.Length == 0)
            {
                return null;
            }

            var serializer = (JsonSerializer)Serializer;
            var entry = serializer.DeserializeObject<ServiceExecutionResult>(Body);

            entry.Result = serializer.DeserializeObject(entry.Result.ToString(), dataType);

            return entry;
        }

        public ServiceInvocationContext GetInvocationContext()
        {
            return new ServiceInvocationContext(this, (types, bytes) => ConvertParamterValues(types, bytes));
        }
    }
}
