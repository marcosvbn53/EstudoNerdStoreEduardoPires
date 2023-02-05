using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services.Base;
using NSE.Bff.Compras.Services.Interface;

namespace NSE.Bff.Compras.Services
{
    public class CatalogoService :Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        }

        public async Task<ItemProdutoDto> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produto/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ItemProdutoDto>(response);
        }
    }
}
