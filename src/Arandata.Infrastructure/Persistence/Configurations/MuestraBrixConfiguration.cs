using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class MuestraBrixConfiguration : IEntityTypeConfiguration<MuestraBrix>
    {
        public void Configure(EntityTypeBuilder<MuestraBrix> builder)
        {
            builder.ToTable("muestra_brix");
            builder.HasKey(x => x.Id).HasName("PK_muestra_brix");
            builder.Property(x => x.Id).HasColumnName("id_muestrabrix");
            builder.Property(x => x.CosechaId).HasColumnName("id_cosecha");
            builder.Property(x => x.FechaMuestreo).HasColumnName("fecha_muestreo").IsRequired();

            builder.HasOne(x => x.Cosecha).WithMany(c => c.MuestrasBrix).HasForeignKey(x => x.CosechaId).HasConstraintName("FK_muestrabrix_cosecha");
        }
    }
}
