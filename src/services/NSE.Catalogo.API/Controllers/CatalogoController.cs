using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using NSE.Catalogo.API.Models.Interfaces;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.API.Controllers
{
    [Authorize]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
        //Mesmo com a controller trancada vamos permitir que qualquer pessoa
        //possa ver os produtos
        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        //Estamos retornando a propria entidade, muitas vezes isso não é uma boa pratica
        //dependendo da sua entidade, se ela for simples não tem problema, porém se ela tiver 
        //mais informações do que você deseja mostrar o ideal é retornar um objeto response
        //que represente sua entidade
        //===========================================
        //Eu não quero que ele esteja apenas autenticado para poder ver o produto detalhe
        //a pessoa tem que ter a claim
        //[ClaimsAuthorize("Catalogo", "Ler")]
        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            //throw new Exception("Erro");
            return await _produtoRepository.ObterPorId(id);
        }
    }
}
