using Ribe.Rpc.Core.Service.Address;
using System;

namespace Ribe.Rpc
{
    /// <summary>
    /// Rpc Exeption
    /// </summary>
    public class RpcException : Exception
    {
        /// <summary>
        /// Request Id
        /// </summary>
        public object RequestId { get; }

        /// <summary>
        /// Client Address
        /// </summary>
        public ServiceAddress Client { get; }

        /// <summary>
        /// Server Address
        /// </summary>
        public ServiceAddress Server { get; }

        public RpcException(string error, object requestId, ServiceAddress client, ServiceAddress server)
            : this(error, requestId, client)
        {
            Server = server;
        }

        public RpcException(string error, object requestId, ServiceAddress client)
        {
            Client = client;
        }

        public RpcException(string error, object requestId)
         : this(error)
        {
            RequestId = requestId;
        }

        public RpcException(string error) : base(error)
        {

        }
    }
}
