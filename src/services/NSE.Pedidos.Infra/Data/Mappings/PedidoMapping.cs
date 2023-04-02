using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pedidos.Dominio.Pedidos;

namespace NSE.Pedidos.Infra.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(px => px.Id);

            builder.OwnsOne(pe => pe.Endereco, px => 
            {
                px.Property(pw => pw.Logradouro).HasColumnName("Logradouro");

                px.Property(pw => pw.Numero).HasColumnName("Numero");

                px.Property(pw => pw.Complemento).HasColumnName("Complemento");

                px.Property(pw => pw.Bairro).HasColumnName("Bairro");

                px.Property(pw => pw.Cep).HasColumnName("CEP");

                px.Property(pw => pw.Cidade).HasColumnName("Cidade");

                px.Property(pw => pw.Estado).HasColumnName("Estado");
            });

            builder.Property(c => c.Codigo).HasDefaultValueSql("NEXT VALUE FOR MinhaSequencia");

            builder.HasMany(pr => pr.PedidoItems)
                .WithOne(pt => pt.Pedido)
                .HasForeignKey(py => py.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}
