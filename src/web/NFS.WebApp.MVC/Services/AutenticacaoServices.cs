using Microsoft.Extensions.Options;
using NFS.WebApp.MVC.Complementar;
using NFS.WebApp.MVC.Models.Identidade;
using NFS.WebApp.MVC.Models.Response;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;
using NFS.WebApp.MVC.Services.ErrorsResponse;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Services
{
    public class AutenticacaoServices : BaseService, IAutenticacaoServices
    {
        private readonly HttpClient _httpClient;
        public AutenticacaoServices(HttpClient httpClient,
                                    IOptions<AppSettings> appSettings )
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Value.AutenticacaoUrl);
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);
            var json = await response.Content.ReadAsStringAsync();


            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjeto<ResponseResult>(response)
                };
            }

            return await DeserializarObjeto<UsuarioRespostaLogin>(response);                
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContente = ObterConteudo(usuarioRegistro);                

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContente);
            var json = await response.Content.ReadAsStringAsync();

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjeto<ResponseResult>(response) 
                };
            }

            return await DeserializarObjeto<UsuarioRespostaLogin>(response);
        }
    }
}
