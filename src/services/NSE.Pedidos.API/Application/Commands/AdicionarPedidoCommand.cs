using FluentValidation;
using NSE.Core.Messages;
using NSE.Pedidos.API.Application.DTO;

namespace NSE.Pedidos.API.Application.Commands
{
    public class AdicionarPedidoCommand : Command
    {
        // Pedido
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<PedidoItemDTO> PedidoItems { get; set; }

        // Voucher
        public string VoucherCodigo { get; set; }
        public bool VoucherUtilazado { get; set; }
        public decimal Desconto { get; set; }

        // Endereco
        public EnderecoDTO Endereco { get; set; }

        // Cartao
        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public string ExpriracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }

    public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoCommand>
    {
        public AdicionarPedidoValidation()
        {
            RuleFor(px => px.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(px => px.PedidoItems.Count())
                .GreaterThan(0)
                .WithMessage("O pedido precisa ter no mínimo 1 item");

            RuleFor(px => px.ValorTotal)
                .GreaterThan(0)
                .WithMessage("Valor do pedido inválido");

            RuleFor(px => px.NumeroCartao)
                .CreditCard()
                .WithMessage("Número de cartão inválido");

            RuleFor(px => px.NomeCartao)
                .NotNull()
                .WithMessage("Nome do portador do cartão requerido.");

            RuleFor(px => px.CvvCartao.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("Data expiração do cartão requerida.");

        }
    }
}
