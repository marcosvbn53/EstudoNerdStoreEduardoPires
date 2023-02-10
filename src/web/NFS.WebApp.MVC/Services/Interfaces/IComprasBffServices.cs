using NFS.WebApp.MVC.Models;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;

namespace NFS.WebApp.MVC.Services.Interfaces
{
    public interface IComprasBffServices
    {
        Task<CarrinhoViewModel> ObterCarrinho();
        Task<int> ObterQuantidadeCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    }
}
