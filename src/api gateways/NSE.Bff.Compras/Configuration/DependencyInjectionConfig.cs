using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services;
using NSE.Bff.Compras.Services.Interface;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.Usuario;
using Polly;

namespace NSE.Bff.Compras.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserHttpAcesso, UserHttpAcesso>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();            

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtension.EsperarTentar())
                .AddTransientHttpErrorPolicy(px => px.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICarrinhoService, CarrinhoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtension.EsperarTentar())
                .AddTransientHttpErrorPolicy(px => px.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IPedidoService, PedidoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtension.EsperarTentar())
                .AddTransientHttpErrorPolicy(px => px.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        }
    }
}
