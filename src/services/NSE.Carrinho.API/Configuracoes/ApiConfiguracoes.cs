using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Carrinho.API.Configuracoes
{
    public static class ApiConfiguracoes
    {
        public static void AddApiConfiguracoes(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CarrinhoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

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
            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UsarIdentityConfiguracao();

            app.MapControllers();

        }
    }
}
