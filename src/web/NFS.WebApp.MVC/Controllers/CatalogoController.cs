using Microsoft.AspNetCore.Mvc;
using NFS.WebApp.MVC.Controllers.Base;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Controllers
{
    public class CatalogoController : BaseController
    {

        private readonly ICatalogoService _catalogoService;

        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        //private readonly ICatalogoServiceRefit _catalogoService;

        //public CatalogoController(ICatalogoServiceRefit catalogoService) 
        //{
        //    _catalogoService = catalogoService;
        //}


        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            var produtos = await _catalogoService.ObterTodos();

            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produto = await _catalogoService.ObterPorId(id);

            return View(produto);
        }
    }
}
