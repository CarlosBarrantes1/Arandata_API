using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class Baya100Configuration : IEntityTypeConfiguration<Baya100>
    {
        public void Configure(EntityTypeBuilder<Baya100> builder)
        {
            builder.ToTable("baya_100");
            builder.HasKey(x => x.Id).HasName("PK_baya_100");
            builder.Property(x => x.Id).HasColumnName("id_baya");
            builder.Property(x => x.Muestra100Id).HasColumnName("id_muestra100");
            builder.Property(x => x.NumeroBaya).HasColumnName("numero_baya");
            builder.Property(x => x.PesoBaya).HasColumnName("peso_baya");
            builder.Property(x => x.DiametroBaya).HasColumnName("diametro_baya");

            builder.HasOne(x => x.Muestra100).WithMany(m => m.Bayas).HasForeignKey(x => x.Muestra100Id).HasConstraintName("FK_baya100_muestra100");
        }
    }
}
