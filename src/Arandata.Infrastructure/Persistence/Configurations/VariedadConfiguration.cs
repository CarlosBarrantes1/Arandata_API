using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class VariedadConfiguration : IEntityTypeConfiguration<Variedad>
    {
        public void Configure(EntityTypeBuilder<Variedad> builder)
        {
            builder.ToTable("variedad");
            builder.HasKey(x => x.Id).HasName("PK_variedad");
            builder.Property(x => x.Id).HasColumnName("id_variedad");
            builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            builder.Property(x => x.DensidadPlantas).HasColumnName("densidad_plantas").IsRequired();
            builder.Property(x => x.PlantasPorVariedad).HasColumnName("plantas_por_variedad");
        }
    }
}
