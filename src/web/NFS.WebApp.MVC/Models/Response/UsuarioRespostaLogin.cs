using NFS.WebApp.MVC.Models.Identidade;
using NFS.WebApp.MVC.Models.Response.ResponseErrors;

namespace NFS.WebApp.MVC.Models.Response
{
    public class UsuarioRespostaLogin
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }
        public ResponseResult ResponseResult { get; set; }
    }
}
