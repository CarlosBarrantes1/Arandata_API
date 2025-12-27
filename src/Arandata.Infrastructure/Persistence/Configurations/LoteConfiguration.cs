using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class LoteConfiguration : IEntityTypeConfiguration<Lote>
    {
        public void Configure(EntityTypeBuilder<Lote> builder)
        {
            builder.ToTable("lote");
            builder.HasKey(x => x.Id).HasName("PK_lote");
            builder.Property(x => x.Id).HasColumnName("id_lote");
            builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            builder.Property(x => x.VariedadId).HasColumnName("id_variedad");
            builder.Property(x => x.FechaSiembra).HasColumnName("fecha_siembra");
            builder.Property(x => x.PlantasTotales).HasColumnName("plantas_totales");
            builder.Property(x => x.Hectareas).HasColumnName("hectareas");

            builder.HasOne(x => x.Variedad).WithMany(v => v.Lotes).HasForeignKey(x => x.VariedadId).HasConstraintName("FK_lote_variedad");
        }
    }
}
