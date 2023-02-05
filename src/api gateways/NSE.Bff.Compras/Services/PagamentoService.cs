using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services.Base;
using NSE.Bff.Compras.Services.Interface;

namespace NSE.Bff.Compras.Services
{
    public class PagamentoService :Service, IPagamentoService
    {
        public readonly HttpClient _httpClient;

        public PagamentoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
        }
    }
}
