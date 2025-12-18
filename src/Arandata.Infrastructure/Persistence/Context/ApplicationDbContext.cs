using Arandata.Domain.Entities;
using Arandata.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Arandata.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Removed unused entities
        public DbSet<Arandata.Domain.Entities.Variedad> Variedades { get; set; } = null!;
        public DbSet<Arandata.Domain.Entities.Lote> Lotes { get; set; } = null!;
        public DbSet<Arandata.Domain.Entities.Cosecha> Cosechas { get; set; } = null!;
        public DbSet<Arandata.Domain.Entities.Muestra100> Muestras100 { get; set; } = null!;
        public DbSet<Arandata.Domain.Entities.Baya100> Bayas100 { get; set; } = null!;
        public DbSet<Arandata.Domain.Entities.MuestraBrix> MuestrasBrix { get; set; } = null!;
        public DbSet<Arandata.Domain.Entities.BayaBrix> BayasBrix { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Removed unused configurations
            // Domain agricultural entities - explicit configurations
            modelBuilder.ApplyConfiguration(new VariedadConfiguration());
            modelBuilder.ApplyConfiguration(new LoteConfiguration());
            modelBuilder.ApplyConfiguration(new CosechaConfiguration());
            modelBuilder.ApplyConfiguration(new Muestra100Configuration());
            modelBuilder.ApplyConfiguration(new Baya100Configuration());
            modelBuilder.ApplyConfiguration(new MuestraBrixConfiguration());
            modelBuilder.ApplyConfiguration(new BayaBrixConfiguration());
        }
    }
}
