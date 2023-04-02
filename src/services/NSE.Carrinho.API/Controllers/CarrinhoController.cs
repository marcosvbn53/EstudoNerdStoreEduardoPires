using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Model;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Controllers
{
    [Authorize]
    public class CarrinhoController : MainController
    {
        private readonly IUserHttpAcesso _user;
        private readonly CarrinhoContext _context;

        public CarrinhoController(IUserHttpAcesso user, CarrinhoContext context)
        {
            _user = user;
            _context = context;
        }

        [HttpGet("carrinho")]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
        }

        [HttpPost("carrinho")]
        public async Task<IActionResult> AdcionarItemCarrinho(CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();

            if(carrinho == null)            
                ManipularNovoCarrinho(item);            
            else            
                ManipularCarrinhoExistente(carrinho, item);

            
            if (!OperacaoValida()) return CustomResponse();

            await PersistirDados();

            return CustomResponse();
        }

        [HttpPut("carrinho/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);
            if (itemCarrinho == null) return CustomResponse();

            carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            _context.CarrinhoItens.Update(itemCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        [HttpDelete("carrinho/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var carrinho = await ObterCarrinhoCliente();

            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);
            if (itemCarrinho == null) return CustomResponse();

            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            _context.CarrinhoItens.Remove(itemCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        [HttpPost]
        [Route("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(Voucher voucher)
        {
            var carrinho = await ObterCarrinhoCliente();

            carrinho.AplicarVoucher(voucher);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }


        private async Task<CarrinhoCliente> ObterCarrinhoCliente()
        {
            return await _context?.CarrinhoCliente
                .Include(px => px.Itens)
                .FirstOrDefaultAsync(px => px.ClienteId == _user.ObterUserId());
        }
        private void ManipularNovoCarrinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_user.ObterUserId());
            carrinho.AdicionarItem(item);

            ValidarCarrinho(carrinho);
            _context.CarrinhoCliente.Add(carrinho);
        }
        private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var produtoItemExistence = carrinho.CarrinhoItemExistente(item);

            carrinho.AdicionarItem(item);
            ValidarCarrinho(carrinho);

            if (produtoItemExistence)
            {
                _context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
            }
            else
            {
                _context.CarrinhoItens.Add(item);
            }

            _context.CarrinhoCliente.Update(carrinho);
        }
        private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
        {
            if(item != null && produtoId != item.ProdutoId)
            {
                AdicionarErroProcessamento("O item não corresponde ao informado");
                return null;
            }

            if(carrinho == null)
            {
                AdicionarErroProcessamento("Carrinho não encontrado");
                return null;
            }

            var itemCarrinho = await _context.CarrinhoItens
                .FirstOrDefaultAsync(px => px.CarrinhoId == carrinho.Id && px.ProdutoId == produtoId);

            if(itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
            {
                AdicionarErroProcessamento("O item não está no carrinho");
                return null;
            }

            return itemCarrinho;
        }
        private async Task PersistirDados()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
        }

        private bool ValidarCarrinho(CarrinhoCliente carrinho)
        {
            if (carrinho.EhValido()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(px => AdicionarErroProcessamento(px.ErrorMessage));
            return false;
        }
    }
}
