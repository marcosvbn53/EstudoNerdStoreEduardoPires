using Microsoft.Extensions.Options;
using NFS.WebApp.MVC.Complementar;
using NFS.WebApp.MVC.Models;
using NFS.WebApp.MVC.Services.ErrorsResponse;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Services
{
    public class CatalogoService : BaseService,ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
            _httpClient = httpClient;
        }
        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
            TratarErrosResponse(response);
            return await DeserializarObjeto<ProdutoViewModel>(response);
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos/");

            TratarErrosResponse(response);

            return await DeserializarObjeto<IEnumerable<ProdutoViewModel>>(response);
        }
    }
}
