using Ribe.Core;
using System.Collections.Generic;

namespace Ribe.Messaging
{
    public class ResponseMessage
    {
        public Dictionary<string, string> Headers { get; }

        public Result Result { get; }

        public ResponseMessage(Dictionary<string, string> headers, Result result)
        {
            Headers = headers;
            Result = result;
        }
    }
}
