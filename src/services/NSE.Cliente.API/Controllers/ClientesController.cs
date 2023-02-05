using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Data;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Clientes.API.Controllers
{
    [Route("api/[controller]")]    
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Index() 
        {
            //O código abaixo é um exemplo contudo
            //A criação de clientes será feita a partir da API de Identidade que
            //irá enviar os dados a serem inseridos.            
            
            var resultado = _mediatorHandler.EnviarComando(
                new RegistrarClienteCommand(Guid.NewGuid(), "Marcos Nascimento", "marcos.ti.an53@gmail.com", "00000000191"));

            return CustomResponse(resultado);
        }
    }
}
