using Ribe.Core.Service.Address;
using System.Collections.Generic;

namespace Ribe.Rpc.Routing
{
    public class RoutingEntry
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务路径
        /// </summary>
        public string ServicePath { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public ServiceAddress Address { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        public Dictionary<string, string> Descriptions { get; set; }

        public RoutingEntry()
        {
            Descriptions = new Dictionary<string, string>();
        }
    }
}
