using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Mediator;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Data
{
    public class ClientesContext : DbContext , IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;

            ///Os recursos abaixo foram desabilitados pois para nossa implementação eles não são necessários e
            ///só iriam atrapalhar, podiamos desabilita-los em um método porem o código ficaria redundante pois nos
            ///precisariams repetilo em vários métodos.

            //Aqui eu desabilito o Tracking em todas as instancia que venham a existir desta classe
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //Aqui desabilitamos o a detectação automatica de alterações 
            //ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Endereco { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();
            //Essa configuração para pegar toda propriedade que passar sem uma definição de size desta forma 
            //não permitimos que o campo string seja criado como varchar(max)
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");


            //Essa configuração desabilita a deleção em cascata, desta forma eu não sou obrigado a sair
            //deletando diversos dados no banco para remover um cliente, claro que isso tem consequências.
            foreach (var relationalship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationalship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
        }


        public async Task<bool> Commit()
        {
            //var sucesso = base.SaveChanges() > 0;
            var sucesso = await base.SaveChangesAsync() > 0;
            if (sucesso)
            {
                //Aqui vamos lançar uma lista de eventos.
                await _mediatorHandler.PublicarEventos(this);
            }
            return sucesso;
        }
    }



}
