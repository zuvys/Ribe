using Ribe.Rpc;

namespace Ribe.DotNetty
{
    public static class RpcBuilderDotNettyExtension
    {
        public static IRpcBuilder AddDotNetty(this IRpcBuilder rpcBuilder)
        {
            //inject DotNetty
            return rpcBuilder;
        }
    }
}
