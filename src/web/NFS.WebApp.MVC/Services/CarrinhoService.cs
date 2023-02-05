using Microsoft.Extensions.Options;
using NFS.WebApp.MVC.Complementar;
using NFS.WebApp.MVC.Models;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;
using NFS.WebApp.MVC.Services.ErrorsResponse;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Services
{
    public class CarrinhoService : BaseService, ICarrinhoService
    {
        private readonly HttpClient _httpClient;

        public CarrinhoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/carrinho/");
            TratarErrosResponse(response);
            return await DeserializarObjeto<CarrinhoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemProdutoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PostAsync("/carrinho/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjeto<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PutAsync($"/carrinho/{produto.ProdutoId}", itemContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjeto<ResponseResult>(response);

            return RetornoOk();
        }        

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");
            if (!TratarErrosResponse(response)) return await DeserializarObjeto<ResponseResult>(response);

            return RetornoOk();
        }
    }
}
