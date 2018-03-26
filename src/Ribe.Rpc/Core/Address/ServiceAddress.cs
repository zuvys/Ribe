using System.Net;
using System;

namespace Ribe.Rpc.Core.Service.Address
{
    public class ServiceAddress
    {
        public int Port { get; set; }

        public string Ip { get; set; }

        public ServiceAddress()
        {

        }

        public ServiceAddress(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        public EndPoint ToEndPoint()
        {
            if (!IPAddress.TryParse(Ip, out var address))
            {
                throw new NotSupportedException($"the {Ip} is not a valid ip");
            }

            return new IPEndPoint(address, Port);
        }

        public override string ToString()
        {
            return $"{Ip}:{Port}";
        }
    }
}