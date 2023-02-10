using NSE.WebAPI.Core.Usuario;
using System.Net.Http.Headers;

namespace NSE.Bff.Compras.Extensions
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IUserHttpAcesso _userHttpAcesso;

        public HttpClientAuthorizationDelegatingHandler(IUserHttpAcesso userHttpAcesso)
        {
            _userHttpAcesso = userHttpAcesso;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _userHttpAcesso.ObterHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }

            var token = _userHttpAcesso.ObterUserToken();

            /*
            if(string.IsNullOrWhiteSpace(token) && authorizationHeader.Count() > 0)            
                token = authorizationHeader[0].Replace("Bearer ", "");
            */
            if(token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
