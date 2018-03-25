using Microsoft.Extensions.DependencyInjection;
using Ribe.Codecs;
using Ribe.Messaging;
using Ribe.Rpc.Json.Codecs;
using Ribe.Rpc.Json.Messaging;
using Ribe.Rpc.Json.Serialize;
using Ribe.Serialize;
using System;

namespace Ribe.Rpc.Json
{
    public static class ServiceProviderExtension
    {
        public static IServiceProvider UseJson(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<IMessageFormatterManager>().AddFormatter(new JsonMessageFormatter());
            serviceProvider.GetRequiredService<ISerializerManager>().AddSerializer(new JsonSerializer());
            serviceProvider.GetRequiredService<IEncoderManager>().AddEncoder(new JsonEncoder());
            serviceProvider.GetRequiredService<IDecoderManager>().AddDecoder(new JsonDecoder());

            return serviceProvider;
        }
    }
}
