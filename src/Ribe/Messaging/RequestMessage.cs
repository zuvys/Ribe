using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class RequestMessage
    {
        public object[] ParamterValues { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public RequestMessage(Dictionary<string, string> headers, object[] paramterValues)
        {
            Headers = headers;
            ParamterValues = paramterValues;
        }
    }
}
