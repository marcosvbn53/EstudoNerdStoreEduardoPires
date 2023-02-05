using Microsoft.AspNetCore.Mvc;
using NFS.WebApp.MVC.Services.Interfaces;

namespace NFS.WebApp.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly ICarrinhoService _carrinhoService;

        public CarrinhoViewComponent(ICarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _carrinhoService.ObterCarrinho() ?? new Models.CarrinhoViewModel());
        }
    }
}
