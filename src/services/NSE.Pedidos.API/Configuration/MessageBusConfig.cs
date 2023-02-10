using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Pedidos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddMessageBus(Configuration.GetMessageQueueConnection("MessageBus"));
        }
    }
}
