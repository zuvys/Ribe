using Microsoft.Extensions.DependencyInjection;
using Ribe.Rpc.Core.Runtime.Client.ServiceProxy;
using Ribe.Rpc.Core.Runtime.Server;
using System;

namespace Ribe.Rpc
{
    public interface IRpcBuilder
    {
        IServiceCollection ServiceCollection { get; }
    }

    public interface IRpcServerBuilder : IRpcBuilder
    {
        IServiceHostFactory BuildServiceHostFacotry();
    }

    public interface IRpcClientBuilder : IRpcBuilder
    {
        IServiceProxyFactory BuildServiceProxyFacotry();
    }

    public class RpcClientBuilder : IRpcClientBuilder
    {
        public IServiceProvider ServiceProvider { get; set; }

        public IServiceCollection ServiceCollection { get; }

        public Action<IServiceProvider> ConfigurationBuilder { get; }

        public RpcClientBuilder(Action<IServiceProvider> configurationBuilder)
        {
            ServiceCollection = new ServiceCollection();
            ConfigurationBuilder = configurationBuilder;
        }

        public IServiceProxyFactory BuildServiceProxyFacotry()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
            ConfigurationBuilder(ServiceProvider);

            return ServiceProvider.GetRequiredService<IServiceProxyFactory>();
        }
    }

    public class RpcServerBuilder : IRpcServerBuilder
    {
        public IServiceProvider ServiceProvider { get; set; }

        public IServiceCollection ServiceCollection { get; }

        public Action<IServiceProvider> ConfigurationBuilder { get; }

        public RpcServerBuilder(Action<IServiceProvider> configurationBuilder)
        {
            ServiceCollection = new ServiceCollection();
            ConfigurationBuilder = configurationBuilder;
        }

        public IServiceHostFactory BuildServiceHostFacotry()
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
            ConfigurationBuilder(ServiceProvider);

            return ServiceProvider.GetRequiredService<IServiceHostFactory>();
        }
    }
}
