using Ribe.Core.Service.Address;
using System;
using System.Collections.Generic;

namespace Ribe.Rpc.Core.Runtime.Client.Selector
{
    public class RandomServiceAddressSelector : IServiceAddressSelector
    {
        public ServiceAddress Select(List<ServiceAddress> hosts, RequestContext req)
        {
            var index = new Random(DateTime.Now.Millisecond).Next(0, hosts.Count * 100) % hosts.Count;

            return hosts[index];
        }
    }
}
