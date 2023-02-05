using NFS.WebApp.MVC.Models;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;

namespace NFS.WebApp.MVC.Services.Interfaces
{
    public interface ICarrinhoService
    {
        Task<CarrinhoViewModel> ObterCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemProdutoViewModel produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    }
}
