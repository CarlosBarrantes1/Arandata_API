using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class Muestra100Configuration : IEntityTypeConfiguration<Muestra100>
    {
        public void Configure(EntityTypeBuilder<Muestra100> builder)
        {
            builder.ToTable("muestra_100");
            builder.HasKey(x => x.Id).HasName("PK_muestra_100");
            builder.Property(x => x.Id).HasColumnName("id_muestra100");
            builder.Property(x => x.CosechaId).HasColumnName("id_cosecha");
            builder.Property(x => x.FechaMuestreo).HasColumnName("fecha_muestreo").IsRequired();

            builder.HasOne(x => x.Cosecha).WithMany(c => c.Muestras100).HasForeignKey(x => x.CosechaId).HasConstraintName("FK_muestra100_cosecha");
        }
    }
}
