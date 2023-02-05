using NFS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NFS.WebApp.MVC.Models.Identidade
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Nome completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CPF")]
        [Cpf]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senha não conferem.")]
        [DisplayName("Confirme sua senha")]
        public string SenhaConfirmacao { get; set; }
    }
}
