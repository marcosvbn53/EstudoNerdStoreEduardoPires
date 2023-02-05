using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //Obtendo os dados provenite do arquivo json AppSettings
            var appSettingsSection = configuration.GetSection("AppSettings");
            //Preparando a Injeção de dependencia da class AppSettings 
            services.Configure<AppSettings>(appSettingsSection);

            //Obtendo a instancia da dependencia AppSettings
            var appSettings = appSettingsSection.Get<AppSettings>();

            //Obterndo a chave secreta da instancia de AppSettings que foi obtido
            //através do arquivo de configuração AppSettings
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });
            //Fim da implementação
        }

        public static IApplicationBuilder UsarIdentityConfiguracao(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

    }
}
