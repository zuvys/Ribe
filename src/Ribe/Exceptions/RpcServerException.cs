using System;

namespace Ribe
{
    public class RpcServerException : Exception
    {
        public object Id { get; }

        public RpcServerException(string message, object id)
           : this(message)
        {
            Id = id;
        }

        public RpcServerException(string message) : base(message)
        {

        }
    }
}
