using FluentValidation.Results;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus.Interface;

namespace NSE.Clientes.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegistroClienteIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();           
            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                    await RegistrarCliente(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        //Tento me conectar e renovo a subscription para continuar me comunicando
        private void OnConnect(object? sender, EasyNetQ.ConnectedEventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent messagem)
        {
            var clienteCommand = new RegistrarClienteCommand(messagem.Id, messagem.Nome, messagem.Email, messagem.Cpf);

            ValidationResult sucesso;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.EnviarComando(clienteCommand);
            }
            return new ResponseMessage(sucesso);
        }
    }
}
