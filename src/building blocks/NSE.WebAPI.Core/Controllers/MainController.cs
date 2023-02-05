using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Comunication.ResponseErrors;

namespace NSE.WebAPI.Core.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        public ICollection<string> Erros = new List<string>();
        protected ActionResult CustomResponse(object result = null)
        {

            if (OperacaoValida())
            {
                return Ok(result);
            }

            //RFC = Request for Comments
            //RFC = Pedido de Comentários
            //IETF = Internet Engineering Task Force
            //IETF = Força-Tarefa de Engenharia da Internet
            //Quando você implementa o retorno de um ValidationProblemDatils, você está aplicando
            //a pratica recomendada na hora de trabalhar com uma API RFC 7807 IETF
            //Está pratica esta na documentação da Microsoft e na RFC citada acima.
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(px => px.Errors);
            foreach (var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult modelState)
        {
            modelState.Errors.ForEach(px => { AdicionarErroProcessamento(px.ErrorMessage); });
            return CustomResponse();
        }

        protected ActionResult CustomResponse(ResponseResult resposta)
        {
            ResponsePossuiErros(resposta);

            return CustomResponse();
        }

        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

            foreach(var mensagem in resposta.Errors.Mensagens)
            {
                AdicionarErroProcessamento(mensagem);
            }

            return true;
        }

        protected bool OperacaoValida()
        {
            return !Erros.Any();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }

        protected void LimparErrosProcessamento()
        {
            Erros.Clear();
        }

    }
}
