using NSE.Clientes.API.Services;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Clientes.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //Essa a classe de extensão que criamos de injeção de dependência
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                //Esse AddHostedService ele Registra uma instancia de RegistroClienteIntegrationHandler
                //porém o ciclo da injeção de dependência deste servço é Singleton 
                .AddHostedService<RegistroClienteIntegrationHandler>();            
        }
    }
}
