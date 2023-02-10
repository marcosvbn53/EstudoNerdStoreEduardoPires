using NSE.Bff.Compras.Models;
using NSE.Core.Comunication.ResponseErrors;

namespace NSE.Bff.Compras.Services.Interface
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);
        Task<PedidoDTO> ObterUltimoPedido();
        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
}
