using System;

namespace Ribe
{
    public class RpcException : Exception
    {
        public string RequestId { get; }

        public RpcException(string message, string requestId)
           : this(message)
        {
            RequestId = requestId;
        }

        public RpcException(string message) : base(message)
        {

        }
    }
}
