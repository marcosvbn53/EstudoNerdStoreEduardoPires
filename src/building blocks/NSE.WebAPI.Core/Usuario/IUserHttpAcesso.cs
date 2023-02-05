using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Usuario
{
    public interface IUserHttpAcesso
    {
        string Name { get; }
        Guid ObterUserId();
        string ObterUserEmail();
        string ObterUserToken();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaims();
        //Esse método não necessáriamente precisava estar aqui porem vai nos ajudar
        HttpContext ObterHttpContext();
    }
}
