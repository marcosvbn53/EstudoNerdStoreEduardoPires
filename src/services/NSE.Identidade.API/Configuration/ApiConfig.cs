using NSE.Identidade.API.Bll;

namespace NSE.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AdicionarConfiguracoesDaApi(this IServiceCollection services)
        {
            services.AddScoped<AutenticacaoJwtBll>();

            services.AddEndpointsApiExplorer();

            services.AddControllers();
            return services;
        }

        public static IApplicationBuilder UsarConfiguracoesDaApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UsarIdentityConfiguracao(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;
        }
    }
}
