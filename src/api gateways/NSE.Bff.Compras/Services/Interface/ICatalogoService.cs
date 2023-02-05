using NSE.Bff.Compras.Models;

namespace NSE.Bff.Compras.Services.Interface
{
    public interface ICatalogoService
    {
        Task<ItemProdutoDto> ObterPorId(Guid id);
    }
}
