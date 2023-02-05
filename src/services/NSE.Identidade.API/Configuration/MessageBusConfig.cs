using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Identidade.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //Essa a classe de extensão que criamos de injeção de dependência
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
        }
    }
}
