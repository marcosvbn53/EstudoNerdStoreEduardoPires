using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.WebAPI.Core.Identidade;
using System.Text;

namespace NSE.Identidade.API.Configuration
{
    public static class IdentityConfig
    {

        public static IServiceCollection AdicionarIdentityConfiguracao(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Acesso ao banco de dados onde se encontram os dados de acesso dos usuários
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Inico da implementação básica do serviço de entidade
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                //Nesta parte fazemos a conversão do 
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // JWT 
            services.AddJwtConfiguration(configuration);                       

            return services;
        }

        public static IApplicationBuilder UsarIdentityConfiguracao(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
