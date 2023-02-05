using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Identidade
{
    public class CustomAuthorize
    {
        //Este método a partir do contexto da requisição e com base no nome da claims e no valor da claims
        //eu valido primeiro se o usuário está autenticado e depois com base nas claims deste usuário eu verifico 
        //se ele possui um claims com base no nome da claims que ele recebe no parâmetro claimName e essa claim
        //contem o valor que eu estou esperando é igual
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                   context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
    }

    //Vamos precisar de outra implementação que é faz parte da implementação acima
    //Vamos criar uma class que servira de atribuito, o mesmo deverá decorar os métodos cujo 
    //acesso deve ser restrito a quem term determinadas permissões
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string ClaimValue) 
            : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, ClaimValue) };
        }
    }
    /// <summary>
    /// Criamos então esta class que implementa a interface IAuthorizationFIlter
    /// </summary>
    public class RequisitoClaimFilter:IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Verifico se ele está autenticado se não estiver retorno o erro 401 dizendo 
            //que não sei quem é essa pessoa que está querendo acesso
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            //Caso ele esteja autenticado então eu verifico se essa pessoa tem a claim no caso o seu valo
            //Se ele não tem eu retorno dizendo que ele não tem a claim e retorno o erro 
            if(!CustomAuthorize.ValidarClaimsUsuario(context.HttpContext,_claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }


}
