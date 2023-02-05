using NFS.WebApp.MVC.Configuration;

namespace NFS.WebApp.MVC
{
    public class Program
    {

        public static IConfiguration Configuration { get; set; }
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var builderConfiguration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();


            if (builder.Environment.IsDevelopment())
            {
                builderConfiguration.AddUserSecrets<Program>();
            }
            Configuration = builderConfiguration.Build();

            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);

            app.Run();
        }


        public static void ConfigureServices(WebApplicationBuilder services)
        {
            services.Services.AdicionarConfiguracaoIdentity();
            services.Services.AdicionarMvcConfiguracao(Configuration);
            services.Services.RegistrarServicos(Configuration);
        }

        public static void Configure(WebApplication app)
        {
            app.UtilizarMvcConfiguracao();
        }


    }
}