using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NFS.WebApp.MVC.Controllers.Base;
using NFS.WebApp.MVC.Models.Identidade;
using NFS.WebApp.MVC.Models.Response;
using NFS.WebApp.MVC.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NFS.WebApp.MVC.Controllers
{
    public class IdentidadeController : BaseController
    {
        private readonly IAutenticacaoServices _autenticacaoServices;


        public IdentidadeController(IAutenticacaoServices autenticacaoServices)
        {
            _autenticacaoServices = autenticacaoServices;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            // API - Registro 
            var resposta = await _autenticacaoServices.Registro(usuarioRegistro);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            // Realizar login APP
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Catalogo");
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(usuarioLogin);

            // API - Registro
            var resposta = await _autenticacaoServices.Login(usuarioLogin);

            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

            // Realizar login na APP
            await RealizarLogin(resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);            
        }

        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            // Remover cookie para que o usuário não seja entendido como usuário logado.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Catalogo");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            //Os dados do usuário ficará dentro do ClaimsPrincipal
            //1º precisamos saber como extrair as informações do token para preencher os ClaimsPrincipal
            //Vamos então instalar um pacote para pode extrair as informações do token.
            //Install-Package Microsoft.AspNetCore.Authentication.JwtBearer

            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperty = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true//Ele será persistente pois vai durar multiplos request dentro do período de validade descrito acima
            };


            //Então quando eu falar para o usuário faça o login chamando o método abaixo SignInAsync
            //Eu vou responder: Você vai trabalhar com cookie
            //Suas claims estão em ClamisIdentity incluse o JWT para relembrar
            //E também as propriedades de autenticação authProperty que são referentes a como eu quero que ele trabalhe 
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperty);
        }


        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
