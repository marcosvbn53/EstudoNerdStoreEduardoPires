using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            //Esta API será acessada por diversos domínios outros endpoints
            //Vamos permitir então acesso total através das regras 
            //AllowAnyOrigin
            //AllowAnyMethod
            //AllowAnyHeader
            //Desta forma eu deixo minha API aberta pra quem quiser consumi-la 
            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });            
        }

        public static void UseApiConfiguration(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //Essa configuração deve ficar entre os Rounting e os
            //nossos endpoints
            app.UsarIdentityConfiguracao();

            app.UseCors("Total");

            app.MapControllers();
        }
    }
}
