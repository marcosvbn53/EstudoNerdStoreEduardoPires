using NSE.Carrinho.API.Data;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Configuracoes
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserHttpAcesso, UserHttpAcesso>();
            services.AddScoped<CarrinhoContext>();
        }
    }
}
