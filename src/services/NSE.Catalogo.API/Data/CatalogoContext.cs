using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using NSE.Core.Messages;

namespace NSE.Catalogo.API.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> options)
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Abaixo vamos configurar para o nosso CatalogoContext ignorar certas propriedades que 
            //não devem ser mapeadas, isso pode acontecer, podemos resoler esse problema na entidade porem uma forma de 
            //resolver esse problema centralizado ess resolução até para que todos possam ver claramente o que foi ignorada 
            //vamos adicionar as linhad de código abaixo
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();


            //Caso eu deixe passar algum campo do tipo string, eu não quero que ele crie como 
            //nvarchar(max) então o tratamento abaixo é para capturar qualquer propriedade que não tenha
            //Essa configuração preestabelecida

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(px => px.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            //Ele vai aplicar as configurações do assembly CatalogoContext
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
