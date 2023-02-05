using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services.Base;
using NSE.Bff.Compras.Services.Interface;

namespace NSE.Bff.Compras.Services
{
    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }
    }
}
