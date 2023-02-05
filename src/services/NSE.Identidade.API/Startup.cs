using NSE.Identidade.API.Configuration;

namespace NSE.Identidade.API
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
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AdicionarIdentityConfiguracao(Configuration);
            services.AdicionarConfiguracoesDaApi();
            services.AdicionarSwaggerConfiguracao();
            services.AddMessageBusConfiguration(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsarSwaggerConfiguracao(env);
            app.UsarConfiguracoesDaApi(env);
        }
    }
}
