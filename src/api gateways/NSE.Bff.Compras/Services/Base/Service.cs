using NSE.Core.Comunication.ResponseErrors;
using System.Text;
using System.Text.Json;

namespace NSE.Bff.Compras.Services.Base
{
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public ResponseResult RetornarOk()
        {
            return new ResponseResult();
        }
    }
}
