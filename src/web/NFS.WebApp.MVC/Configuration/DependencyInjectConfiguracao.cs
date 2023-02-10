using Microsoft.AspNetCore.Mvc.DataAnnotations;
using NFS.WebApp.MVC.Extensions;
using NFS.WebApp.MVC.Services;
using NFS.WebApp.MVC.Services.handlers;
using NFS.WebApp.MVC.Services.Interfaces;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.Usuario;
using Polly;

namespace NFS.WebApp.MVC.Configuration
{
    public static class DependencyInjectConfiguracao
    {
        public static void RegistrarServicos(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserHttpAcesso, UserHttpAcesso>();                      

            services.AddHttpClient<IAutenticacaoServices, AutenticacaoServices>()
                .AddPolicyHandler(PollyExtension.EsperarTentar())
                .AddTransientHttpErrorPolicy(px => px.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            #region

            ///Código sem Retry
            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .AddHttpMessageHandler<HandlerClientAuthorizationDelegationHandler>();
            ///

            //Implementação do Retry
            //Esta politica só será aplicada quando ocorrer erros na requisição então ele tenta várias vezes.
            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .AddHttpMessageHandler<HandlerClientAuthorizationDelegationHandler>()
            //    .AddTransientHttpErrorPolicy(
            //        p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600))
            //    );
            ///Fim da implementação do Retry


            //Vamos adicionar uma policy customizada
            //- Esta é uma forma de criar uma policy 
            //var retryWaitPolicy = HttpPolicyExtensions
            //    .HandleTransientHttpError()
            //    .WaitAndRetryAsync(new[]
            //    {
            //        TimeSpan.FromSeconds(1),
            //        TimeSpan.FromSeconds(5),
            //        TimeSpan.FromSeconds(10)
            //    }, (outcome, timespan, retryCount, context) =>
            //    {
            //        Console.ForegroundColor = ConsoleColor.Blue;
            //        Console.WriteLine($"Tentando pela {retryCount} vez!");
            //        Console.ForegroundColor = ConsoleColor.White;
            //    });

            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .AddHttpMessageHandler<HandlerClientAuthorizationDelegationHandler>()
            //    .AddPolicyHandler(retryWaitPolicy);
            //Fim da implementação Retry com police especializada.

            #endregion

            //services.AddTransient<HandlerClientAuthorizationDelegationHandler>();
            services.AddScoped<HandlerClientAuthorizationDelegationHandler>();

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HandlerClientAuthorizationDelegationHandler>()
                .AddPolicyHandler(PollyExtension.EsperarTentar())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IComprasBffServices, ComprasBffServices>()
                .AddHttpMessageHandler<HandlerClientAuthorizationDelegationHandler>()
                .AddPolicyHandler(PollyExtension.EsperarTentar())
                .AddTransientHttpErrorPolicy(px => px.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


            #region Testes com Refit
            ///A implementação abaixo é para utilizar o Refit
            //services.AddHttpClient("Refit", options =>
            //{
            //    options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            //})
            //  .AddHttpMessageHandler<HandlerClientAuthorizationDelegationHandler>()
            //  .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
            ///////////////////Fim da implementação com o Refit////////////////////////

            #endregion

        }
    }



}
