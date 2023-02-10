using NSE.Pedidos.API.Configuration;
using NSE.WebAPI.Core.Identidade;
using MediatR;

namespace NSE.Pedidos.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddApiConfiguration(Configuration);

            services.AddJwtConfiguration(Configuration);

            services.AddSwaggerConfiguration();

            services.AddMediatR(typeof(Startup));

            services.RegistroServices();

            services.AddMessageBusConfiguration(Configuration);
        }

        public void Configure(WebApplication app)
        {

            app.UseSwaggerConfiguration();
            app.UseApiConfiguration();
        }
    }
}
