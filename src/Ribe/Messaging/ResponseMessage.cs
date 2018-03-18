using Ribe.Core;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class ResponseMessage
    {
        public Response Result { get; }

        public Dictionary<string, string> Headers { get; }

        public ResponseMessage(Dictionary<string, string> headers, Response result)
        {
            Headers = headers;
            Result = result;
        }
    }
}
