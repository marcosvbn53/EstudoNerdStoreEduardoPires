using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services.Base;
using NSE.Bff.Compras.Services.Interface;
using NSE.Core.Comunication.ResponseErrors;
using System.Net;

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

        public Task<ResponseResult> FinalizarPedido(PedidoDTO pedido)
        {
            return null;//throw new NotImplementedException();
        }

        public Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
        {
            return null;//throw new NotImplementedException();
        }

        public Task<PedidoDTO> ObterUltimoPedido()
        {
            return null;//throw new NotImplementedException();
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"/voucher/{codigo}/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VoucherDTO>(response);
        }
    }
}
