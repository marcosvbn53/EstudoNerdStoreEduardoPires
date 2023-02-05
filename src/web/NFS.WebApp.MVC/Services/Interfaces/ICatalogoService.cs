using NFS.WebApp.MVC.Models;
using Refit;

namespace NFS.WebApp.MVC.Services.Interfaces
{
    public interface ICatalogoService
    {
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }

    //Com a implementação abaixo eu não preciso criar uma class com
    //a implementação concreta
    public interface ICatalogoServiceRefit
    {
        //O atributo do tipo Get abaixo pertece ao Refit         
        [Get("/catalogo/produtos")]
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();

        [Get("/catalogo/produtos/{id}")]
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }


}
