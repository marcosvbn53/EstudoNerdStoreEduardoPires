using NSE.WebAPI.Core.Usuario;
using System.Net.Http.Headers;

namespace NFS.WebApp.MVC.Services.handlers
{
    public class HandlerClientAuthorizationDelegationHandler :DelegatingHandler
    {
        private readonly IUserHttpAcesso _user;

        public HandlerClientAuthorizationDelegationHandler(IUserHttpAcesso user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string> { authorizationHeader });
            }

            var token = _user.ObterUserToken();

            if(token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }

    }
}
