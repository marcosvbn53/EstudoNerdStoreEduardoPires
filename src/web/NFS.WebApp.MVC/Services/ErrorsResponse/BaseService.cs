using NFS.WebApp.MVC.Extensions;
using System.Text.Json;
using System.Text;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;

namespace NFS.WebApp.MVC.Services.ErrorsResponse
{
    public abstract class BaseService
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            //Tratamentos com base no StatusCode
            switch ((int)response.StatusCode)
            {
                case 401:  //Não autorizado, não conhece o usuário
                case 403:  //Acesso negado 
                case 404:  //Recurso não encontrado
                case 500:  //Erro de servidor
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400: return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected StringContent ObterConteudo(object dados)
        {
            string resTemp = JsonSerializer.Serialize(dados);

            return new StringContent(JsonSerializer.Serialize(dados), Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializarObjeto<T>(HttpResponseMessage httpResponseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync(), options);
        }

        public ResponseResult RetornoOk()
        {
            return new ResponseResult();
        }
    }
}
