using Microsoft.OpenApi.Models;

namespace NSE.Identidade.API.Configuration
{
    public static class SwaggerConfig
    {

        public static IServiceCollection AdicionarSwaggerConfiguracao(this IServiceCollection services)
        {
            //Essa é definição mais simples 
            //services.AddSwaggerGen();

            //Essa é uma definição explicita de versão da api
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NerdStore Enterprise Identity API",
                    Description = "Está API faz parte do curso ASP.NET Core Enterprise Applications.",
                    Contact = new OpenApiContact() { Name = "Eduardo Pires", Email = "contato@desenvolvimento.io" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/MIT") }
                });
            });
            return services;
        }


        public static IApplicationBuilder UsarSwaggerConfiguracao(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            return app;
        }
    }
}
