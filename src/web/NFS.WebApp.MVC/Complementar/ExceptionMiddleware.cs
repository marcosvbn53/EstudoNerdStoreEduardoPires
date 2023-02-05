using NFS.WebApp.MVC.Extensions;
using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NFS.WebApp.MVC.Complementar
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(CustomHttpRequestException ex)
            {
                HandlerRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch(ValidationApiException ex)
            {
                HandlerRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch(ApiException ex)
            {
                HandlerRequestExceptionAsync(httpContext, ex.StatusCode);
            }   
            catch(BrokenCircuitException)
            {
                HandlerCircuitBreakExceptionAsync(httpContext);
            }
            catch(Exception ex)
            {

            }
        }

        private void HandlerRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            //Quando o usuário tentar acessar uma página que ele não tem acesso a consequência será o redirencionamento para a página de login
            if(statusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private static void HandlerCircuitBreakExceptionAsync(HttpContext context)
        {
            context.Response.Redirect("/sistema-indisponibilidade");
        }
    }
}
