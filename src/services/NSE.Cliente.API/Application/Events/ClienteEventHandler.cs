using MediatR;

namespace NSE.Clientes.API.Application.Events
{
    public class ClienteEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            //Sugestões:
            //Enviar um evento de confirmação 
            //Enviar um email 
            //Auditar dados.
            return Task.CompletedTask;
        }
    }
}
