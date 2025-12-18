using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arandata.Infrastructure.Persistence.Configurations
{
    public class BayaBrixConfiguration : IEntityTypeConfiguration<BayaBrix>
    {
        public void Configure(EntityTypeBuilder<BayaBrix> builder)
        {
            builder.ToTable("baya_brix");
            builder.HasKey(x => x.Id).HasName("PK_baya_brix");
            builder.Property(x => x.Id).HasColumnName("id_baya_brix");
            builder.Property(x => x.MuestraBrixId).HasColumnName("id_muestrabrix");
            builder.Property(x => x.NumeroBaya).HasColumnName("numero_baya");
            builder.Property(x => x.BrixBaya).HasColumnName("brix_baya");

            builder.HasOne(x => x.MuestraBrix).WithMany(m => m.Bayas).HasForeignKey(x => x.MuestraBrixId).HasConstraintName("FK_bayabrix_muestrabrix");
        }
    }
}
