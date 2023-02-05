using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Models;
using NSE.Clientes.API.Models.Interfaces;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            //Se for valido então vamos criar uma instancia de cliente com os dados que vieram 
            //do nosso RegistrarClienteCommand message, que é nossa sacola de dados.
            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

            // Validações de negocio 
            var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

            //Adiantando o comportamento que vai acontecer nas proximas aulas
            //Vamos ter uma validação pra verificar se o cliente existe no banco de dados
            //Exemplo:

            if (clienteExistente != null) //Já existe cliente com o CPF informado
            {
                //Se o cliente já existe no banco ele vai retornar no ValidateResult e não uma 
                //Excecao pois a excecao só será lançada se de fato for uma excecao 
                AdicionarErro("Este CPF já está em uso.");
                return ValidationResult;
            }

            _clienteRepository.Adicionar(cliente);

            //Prepara o evento para ser enviado
            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

            // Persistir no banco!                      
            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}
