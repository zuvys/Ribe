using System.Net;
using System;

namespace Ribe.Core.Service.Address
{
    public class ServiceAddress
    {
        public int Port { get; set; }

        public string Ip { get; set; }

        public EndPoint ToEndPoint()
        {
            if (!IPAddress.TryParse(Ip, out var address))
            {
                throw new NotSupportedException($"the {Ip} is not a valid ip");
            }

            return new IPEndPoint(address, Port);
        }
    }
}