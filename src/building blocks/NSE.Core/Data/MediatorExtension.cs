using Microsoft.EntityFrameworkCore;
using NSE.Core.DomainObjects;
using NSE.Core.Mediator;

namespace NSE.Core.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(px => px.Entity.Eventos != null && px.Entity.Eventos.Any());

            var domainEvents = domainEntities
                .SelectMany(px => px.Entity.Eventos)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvents) =>
                {
                    await mediator.PublicarEvento(domainEvents);
                });

            await Task.WhenAll(tasks);
        }
    }
}
