using Microsoft.AspNetCore.Mvc;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;

namespace NFS.WebApp.MVC.Controllers.Base
{
    public class BaseController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if(resposta?.Errors == null)
                return false;

            if(resposta != null && resposta.Errors.Mensagens.Any())
            {
                resposta.Errors.Mensagens.ForEach(px =>
                {
                    ModelState.AddModelError(string.Empty, px);
                });
                return true;
            }
            return false;
        }

        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
