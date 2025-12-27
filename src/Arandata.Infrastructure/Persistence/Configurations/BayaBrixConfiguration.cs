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
            builder.Property(x => x.MuestraId).HasColumnName("id_muestra");
            builder.Property(x => x.NumeroBaya).HasColumnName("numero_baya");
            builder.Property(x => x.Brix).HasColumnName("brix");

            builder.HasOne(x => x.Muestra).WithMany(m => m.BayasBrix).HasForeignKey(x => x.MuestraId).HasConstraintName("FK_bayabrix_muestra");
        }
    }
}
