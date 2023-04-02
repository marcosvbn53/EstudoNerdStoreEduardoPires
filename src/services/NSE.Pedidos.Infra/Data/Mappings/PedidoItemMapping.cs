using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pedidos.Dominio.Pedidos;

namespace NSE.Pedidos.Infra.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.HasKey(px => px.Id);

            builder.Property(pw => pw.ProdutoNome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.HasOne(pe => pe.Pedido)
                .WithMany(pe => pe.PedidoItems);

            builder.ToTable("PedidoItems");            
        }
    }
}
