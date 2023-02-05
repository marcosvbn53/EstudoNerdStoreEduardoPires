using NSE.Carrinho.API.Configuracoes;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Carrinho.API
{
    public class Startup
    {
        public IConfiguration configuration { get; }
        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if(hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguracoes(configuration);

            services.AddJwtConfiguration(configuration);

            services.AddSwaggerCongiguration();

            services.RegisterServices();
        }

        public void Configure(WebApplication app)
        {
            app.UseSwaggerConfiguration();
            app.UseApiConfiguration();
        }
    }
}
