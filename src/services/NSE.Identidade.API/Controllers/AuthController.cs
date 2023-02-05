using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Messages.Integration;
using NSE.Identidade.API.Bll;
using NSE.Identidade.API.Models.Dto;
using NSE.MessageBus.Interface;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly AutenticacaoJwtBll _autenticacaoJwtBll;
        private IMessageBus _bus;

        public AuthController(AutenticacaoJwtBll autenticacaoJwtBll, IMessageBus bus)
        {
            _autenticacaoJwtBll = autenticacaoJwtBll;
            _bus = bus;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _autenticacaoJwtBll.UserManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                //Aqui vamos chamar o nosso método de integração para criar o registro do cliente, contudo 
                //a requisição que virá abaixo só colocar a nossa solicitação na fila para criar o cliente
                var ClienteResult = await RegistrarCliente(usuarioRegistro);
                //Agora vamos validar se não houve nenhum erro ao criar o cliente, pois se houver algum
                //erro é necessário remover o usuário e repassar os erros encontrados para a aplicação que requisitou a criação.
                if (!ClienteResult.ValidationResult.IsValid)
                {
                    await _autenticacaoJwtBll.UserManager.DeleteAsync(user);
                    return CustomResponse(ClienteResult.ValidationResult);
                }

                //Persistente: define se o usuáio vai ser lembrado
                //O método abaixo comentado foi usado apenas de teste para fazer login 
                //await _signInManager.SignInAsync(user, false);
                return CustomResponse(await _autenticacaoJwtBll.GerarJwt(usuarioRegistro.Email));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _autenticacaoJwtBll.SignInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await _autenticacaoJwtBll.GerarJwt(usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }
            AdicionarErroProcessamento("Usuário ou senha incorretos!");
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _autenticacaoJwtBll.UserManager.FindByEmailAsync(usuarioRegistro.Email);
            //O Identity usa o Guid como string porém não é uma boa pratica 
            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
                Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuario.Email, usuarioRegistro.Cpf);

            //Aqui passamos o tipo que queremos enviar UsuarioRegistradoIntegrationEvent, o
            //tipo de dados que esperamos ResponseMessage, e o dado que queremos enviar propriamente dito é usuarioRegistrado
            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _autenticacaoJwtBll.UserManager.DeleteAsync(usuario);
                throw;
            }                        
        }

    }
}
