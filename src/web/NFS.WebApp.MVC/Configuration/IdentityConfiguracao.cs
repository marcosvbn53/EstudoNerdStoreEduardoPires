using Microsoft.AspNetCore.Authentication.Cookies;

namespace NFS.WebApp.MVC.Configuration
{
    public static class IdentityConfiguracao
    {
        public static void AdicionarConfiguracaoIdentity(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/acesso-negado";
                });
        }


        public static void UsarConfiguracaoIdentity(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

    }
}
