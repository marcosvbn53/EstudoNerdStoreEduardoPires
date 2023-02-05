using NSE.Bff.Compras.Configuration;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Bff.Compras
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

        public void ConfigureService(IServiceCollection services)
        {
            services.AddApiConfiguration(configuration);
            
            services.AddJwtConfiguration(configuration);

            services.AddSwaggerConfiguration();

            services.RegisterServices();
        }

        public void Configure(WebApplication app)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration();
        }
    }
}
