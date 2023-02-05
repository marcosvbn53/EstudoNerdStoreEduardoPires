using MediatR;
using NSE.Clientes.API.Configuration;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Clientes.API
{
    public class Startup
    {
        IConfiguration Configuration { get; }

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

            Configuration = builder.Build();
        }

        public void ConfigureService(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);
            
            services.AddJwtConfiguration(Configuration);

            services.AddSwaggerConfiguration();

            services.AddMediatR(typeof(Startup));

            services.RegisterServices();

            services.AddMessageBusConfiguration(Configuration);
        }

        public void Configure(WebApplication app)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration();
        }

    }
}
