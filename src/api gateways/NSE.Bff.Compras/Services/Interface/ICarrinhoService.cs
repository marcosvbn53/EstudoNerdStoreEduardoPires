using NSE.Bff.Compras.Models;
using NSE.Core.Comunication.ResponseErrors;

namespace NSE.Bff.Compras.Services.Interface
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDto> ObterCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDto produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    }
}
