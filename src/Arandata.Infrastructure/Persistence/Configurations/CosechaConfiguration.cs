using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class CosechaConfiguration : IEntityTypeConfiguration<Cosecha>
    {
        public void Configure(EntityTypeBuilder<Cosecha> builder)
        {
            builder.ToTable("cosecha");
            builder.HasKey(x => x.Id).HasName("PK_cosecha");
            builder.Property(x => x.Id).HasColumnName("id_cosecha");
            builder.Property(x => x.LoteId).HasColumnName("id_lote");
            builder.Property(x => x.FechaCosecha).HasColumnName("fecha_cosecha").IsRequired();
            builder.Property(x => x.KgTotal).HasColumnName("kg_total");
            builder.Property(x => x.KgTotalAcumulado).HasColumnName("kg_total_acumulado");
            builder.Property(x => x.KgPlanta).HasColumnName("kg_planta");
            builder.Property(x => x.KgPlantaAcumulado).HasColumnName("kg_planta_acumulado");
            builder.Property(x => x.DiasDespuesPoda).HasColumnName("dias_despues_poda");

            builder.HasOne(x => x.Lote).WithMany(l => l.Cosechas).HasForeignKey(x => x.LoteId).HasConstraintName("FK_cosecha_lote");
        }
    }
}
