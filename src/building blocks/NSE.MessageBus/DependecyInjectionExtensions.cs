using Microsoft.Extensions.DependencyInjection;
using NSE.MessageBus.Interface;

namespace NSE.MessageBus
{
    public static class DependecyInjectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connection)
        {
            if (string.IsNullOrEmpty(connection))
            {
                throw new ArgumentNullException();
            }

            services.AddSingleton<IMessageBus>(new MessageBus(connection));

            return services;
        }
    }
}
