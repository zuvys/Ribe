namespace Ribe.Rpc.Json
{
    public static class RpcBuilderJsonExtension
    {
        public static IRpcBuilder AddJson(this IRpcBuilder builder)
        {
            //inject Json
            return builder;
        }
    }
}
