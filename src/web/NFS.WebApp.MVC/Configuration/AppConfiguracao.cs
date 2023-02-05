using Microsoft.AspNetCore.Localization;
using NFS.WebApp.MVC.Complementar;
using System.Globalization;

namespace NFS.WebApp.MVC.Configuration
{
    public static class AppConfiguracao
    {
        public static void AdicionarMvcConfiguracao(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.Configure<AppSettings>(configuration);
        }

        public static void UtilizarMvcConfiguracao(this WebApplication app)
        {
            //if (!app.Environment.IsDevelopment())
            //{                
                app.UseDeveloperExceptionPage();
            //}

            //app.UseExceptionHandler("/Home/Error");                
            //Para tratamento de exceção que eu não conseguir interceptar
            app.UseExceptionHandler("/erro/500");

            //Para erros que eu consegui tratar
            app.UseStatusCodePagesWithRedirects("/erro/{0}");

            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UsarConfiguracaoIdentity();


            //Aqui temos acesso a lista de cultura e já definimos uma 
            var suporteCultures = new[] { new CultureInfo("pt-BR") };

            //Aplicamos a cultura que foi definida acima tanto para aplicação quanto para o FrontEnd
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = suporteCultures,
                SupportedUICultures = suporteCultures
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalogo}/{action=Index}/{id?}");            
        }

    }
}
