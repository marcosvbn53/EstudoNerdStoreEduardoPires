using Microsoft.Extensions.Options;
using NFS.WebApp.MVC.Complementar;
using NFS.WebApp.MVC.Models;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;
using NFS.WebApp.MVC.Services.ErrorsResponse;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Services
{
    public class ComprasBffServices : BaseService, IComprasBffServices
    {
        private readonly HttpClient _httpClient;

        public ComprasBffServices(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl);
        }

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/compras/carrinho/");

            TratarErrosResponse(response);

            return await DeserializarObjeto<CarrinhoViewModel>(response);
        }

        public async Task<int> ObterQuantidadeCarrinho()
        {
            var response = await _httpClient.GetAsync("/compras/carrinho-quantidade");

            TratarErrosResponse(response);

            return await DeserializarObjeto<int>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PostAsync("/compras/carrinho/items", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjeto<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjeto<ResponseResult>(response);

            return RetornoOk();
        }        

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");
            if (!TratarErrosResponse(response)) return await DeserializarObjeto<ResponseResult>(response);

            return RetornoOk();
        }        
    }
}
