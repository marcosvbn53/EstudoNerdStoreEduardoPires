using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFS.WebApp.MVC.Controllers.Base;
using NFS.WebApp.MVC.Models;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Controllers
{
    [Authorize]
    public class CarrinhoController : BaseController
    {
        private readonly IComprasBffServices _ComprasBffService;

        #region Antes da implementação do ComprasBff
        //private readonly ICatalogoService _catalogoService;
        //private readonly ICarrinhoService _CarrinhoService;
        #endregion

        public CarrinhoController(
            IComprasBffServices comprasBffService
             /*,ICatalogoService catalogoService
               ,ICarrinhoService carrinhoService
              */)
        {
            _ComprasBffService = comprasBffService;            
            //_CarrinhoService = carrinhoService;
            //_catalogoService = catalogoService;
        }

        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _ComprasBffService.ObterCarrinho());
        }

        [HttpPost]
        [Route("carinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
        {
            #region Antes da implementação do ComprasBFF
            //var produto = await _catalogoService.ObterPorId(itemCarrinho.ProdutoId);

            //ValidarItemCarrinho(produto, itemCarrinho.Quantidade);

            //if (!OperacaoValida()) return View("Index", await _CarrinhoService.ObterCarrinho());

            //itemCarrinho.Nome = produto.Nome;
            //itemCarrinho.Valor = produto.Valor;
            //itemCarrinho.Imagem = produto.Imagem;

            //var resposta = await _CarrinhoService.AdicionarItemCarrinho(itemCarrinho);

            //if (ResponsePossuiErros(resposta)) 
            //    return View("Index", await _ComprasBffService.ObterCarrinho());            
            //return RedirectToAction("Index");            
            #endregion

            #region Após a implementação do ComprasBff
            var resposta = await _ComprasBffService.AdicionarItemCarrinho(itemCarrinho);

            if (ResponsePossuiErros(resposta)) return View("Index", await _ComprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
            #endregion 
        }

        [HttpPost]
        [Route("carrinho/atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            #region Antes da implementação do ComprasBff
            //var produto = await _catalogoService.ObterPorId(produtoId);

            //ValidarItemCarrinho(produto, quantidade);

            //if (!OperacaoValida()) return View("Index", await _CarrinhoService.ObterCarrinho());


            //var itemProduto = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            //var resposta = await _CarrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

            //if (ResponsePossuiErros(resposta)) return View("Index", await _CarrinhoService.ObterCarrinho());

            //return RedirectToAction("Index");
            #endregion

            #region Após A implementação do compras Bff
            var item = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            
            var resposta = await _ComprasBffService.AtualizarItemCarrinho(produtoId, item);

            if (ResponsePossuiErros(resposta)) return View("Index", await _ComprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
            #endregion

        }

        [HttpPost]
        [Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            #region Antes da implementação do comprasBff
            //var produto = await _catalogoService.ObterPorId(produtoId);

            //if(produto == null)
            //{
            //    AdicionarErroValidacao("Produto inexistente!");
            //    return View("Index", await _CarrinhoService.RemoverItemCarrinho(produtoId));
            //}

            //var resposta = await _CarrinhoService.RemoverItemCarrinho(produtoId);

            //if (ResponsePossuiErros(resposta)) return View("Index", await _CarrinhoService.ObterCarrinho());

            //return RedirectToAction("Index");
            #endregion 

            var resposta = await _ComprasBffService.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(resposta)) return View("Index", await _ComprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var resposta = await _ComprasBffService.AplicarVoucherCarrinho(voucherCodigo);

            if (ResponsePossuiErros(resposta)) return View("Index", await _ComprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }


    }
}
