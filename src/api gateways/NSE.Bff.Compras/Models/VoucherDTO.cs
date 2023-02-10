namespace NSE.Bff.Compras.Models
{
    public class VoucherDTO
    {
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string Codigo { get; set; }
        //0 ou 1
        public int TipoDesconto { get; set; }

    }
}
