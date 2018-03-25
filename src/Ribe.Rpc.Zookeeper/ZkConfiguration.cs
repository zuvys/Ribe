namespace Ribe.Rpc.Zookeeper
{
    public class ZkConfiguration
    {
        public string Address { get; set; }

        public string RootPath { get; set; }

        public int SessionTimeout { get; set; }
    }
}
