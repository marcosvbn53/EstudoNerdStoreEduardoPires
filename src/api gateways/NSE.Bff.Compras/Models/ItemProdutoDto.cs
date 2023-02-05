namespace NSE.Bff.Compras.Models
{
    //Esta classe representa um item do catalogo 
    public class ItemProdutoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }  
        public string Imagem { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
