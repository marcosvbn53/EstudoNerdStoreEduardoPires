using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Clientes.API.Models;
using NSE.Core.DomainObjects;

namespace NSE.Clientes.API.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            //Neste mapeamento eu informo que iremos salvar o campo Numero contido no objeto CPF e  
            //não a propriedade CPF
            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Numero)
                    .IsRequired()
                    .HasMaxLength(Cpf.CpfMaxLenght)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({Cpf.CpfMaxLenght})");
            });

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Endereco)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType($"varchar({Email.EnderecoMaxLength})");
            });


            //Relacionamento de 1 para 1 
            //Onde um cliente possui um Endereço, e um Endereço possui um cliente
            builder.HasOne(c => c.Endereco)
                .WithOne(c => c.Cliente);

            builder.ToTable("Clientes");
        }
    }
}
