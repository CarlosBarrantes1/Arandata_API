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
        public DbSet<Muestra> Muestras { get; set; } = null!;
        public DbSet<BayaBrix> BayasBrix { get; set; } = null!;
        public DbSet<BayaDiametro> BayasDiametro { get; set; } = null!;
        public DbSet<BayaPeso> BayasPeso { get; set; } = null!;
        public DbSet<Poda> Podas { get; set; } = null!;
        public DbSet<MuestraTipo> MuestraTipos { get; set; } = null!;

        // Módulo de Seguridad
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<UsuarioRol> UsuarioRoles { get; set; } = null!;
        public DbSet<Modulo> Modulos { get; set; } = null!;
        public DbSet<RolModulo> RolModulos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración Seguridad
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<RolModulo>()
                .HasOne(rm => rm.Rol)
                .WithMany(r => r.RolModulos)
                .HasForeignKey(rm => rm.RolId);

            modelBuilder.Entity<RolModulo>()
                .HasOne(rm => rm.Modulo)
                .WithMany(m => m.RolModulos)
                .HasForeignKey(rm => rm.ModuloId);

            // Domain agricultural entities - explicit configurations
            modelBuilder.ApplyConfiguration(new VariedadConfiguration());
            modelBuilder.ApplyConfiguration(new LoteConfiguration());
            modelBuilder.ApplyConfiguration(new CosechaConfiguration());
            modelBuilder.ApplyConfiguration(new BayaBrixConfiguration());
        }
    }
}
