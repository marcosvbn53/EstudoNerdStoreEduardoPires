using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Usuario
{
    public class UserHttpAcesso : IUserHttpAcesso
    {
        //A interface IHttpContextAccessor é responsavel por obter dados do contexto das minhas requisições
        private readonly IHttpContextAccessor _accessor;
        public UserHttpAcesso(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public bool EstaAutenticado()
        {
            string header = _accessor.HttpContext.Request.Headers["Authorization"];            
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _accessor.HttpContext;
        }

        public string ObterUserEmail()
        {
            //return EstaAutenticado() ? _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email): "";
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public Guid ObterUserId()
        {            
            //return EstaAutenticado() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
            return EstaAutenticado() ? Guid.Parse(_accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)) : Guid.Empty;
        }

        public string ObterUserToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserToken() : "";
        }

        public bool PossuiRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }
    }
}
