using NFS.WebApp.MVC.Models.Identidade;
using NFS.WebApp.MVC.Models.Response;

namespace NFS.WebApp.MVC.Services.Interfaces
{
    public interface IAutenticacaoServices
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
    }
}
