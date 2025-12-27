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

            // Configuración Seguridad - Mapeo exacto al script del usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_usuario");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Correo).HasColumnName("correo");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Activo).HasColumnName("activo");
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("rol");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_rol");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Ignore(e => e.Descripcion); // No existe en el script
            });

            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.ToTable("usuario_rol");
                entity.HasKey(ur => new { ur.UsuarioId, ur.RolId });
                entity.Property(e => e.UsuarioId).HasColumnName("id_usuario");
                entity.Property(e => e.RolId).HasColumnName("id_rol");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.ToTable("modulo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_modulo");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Ignore(e => e.Codigo); // No existe en el script
            });

            modelBuilder.Entity<RolModulo>(entity =>
            {
                entity.ToTable("rol_modulo");
                entity.HasKey(rm => new { rm.RolId, rm.ModuloId });
                entity.Property(e => e.RolId).HasColumnName("id_rol");
                entity.Property(e => e.ModuloId).HasColumnName("id_modulo");
                entity.Property(e => e.PuedeVer).HasColumnName("puede_ver");
                entity.Property(e => e.PuedeCrear).HasColumnName("puede_crear");
                entity.Property(e => e.PuedeEditar).HasColumnName("puede_editar");
                entity.Property(e => e.PuedeEliminar).HasColumnName("puede_eliminar");
            });

            // Mapeo de Entidades Agrícolas al script del usuario
            modelBuilder.Entity<Variedad>(entity =>
            {
                entity.ToTable("variedad");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_variedad");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.DensidadPlantas).HasColumnName("densidad_plantas");
                entity.Property(e => e.PlantasPorVariedad).HasColumnName("plantas_por_variedad");
            });

            modelBuilder.Entity<Lote>(entity =>
            {
                entity.ToTable("lote");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_lote");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.VariedadId).HasColumnName("id_variedad");
                entity.Property(e => e.FechaSiembra).HasColumnName("fecha_siembra");
                entity.Property(e => e.PlantasTotales).HasColumnName("plantas_totales");
                entity.Property(e => e.Hectareas).HasColumnName("hectareas");
            });

            modelBuilder.Entity<Cosecha>(entity =>
            {
                entity.ToTable("cosecha");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_cosecha");
                entity.Property(e => e.LoteId).HasColumnName("id_lote");
                entity.Property(e => e.FechaCosecha).HasColumnName("fecha_cosecha");
                entity.Property(e => e.KgTotal).HasColumnName("kg_total");
                entity.Property(e => e.KgTotalAcumulado).HasColumnName("kg_total_acumulado");
                entity.Property(e => e.KgPlanta).HasColumnName("kg_planta");
                entity.Property(e => e.KgPlantaAcumulado).HasColumnName("kg_planta_acumulado");
                entity.Property(e => e.DiasDespuesPoda).HasColumnName("dias_despues_poda");
            });

            modelBuilder.Entity<Poda>(entity =>
            {
                entity.ToTable("poda");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_poda");
                entity.Property(e => e.LoteId).HasColumnName("id_lote");
                entity.Property(e => e.FechaPoda).HasColumnName("fecha_poda");
            });

            modelBuilder.Entity<Muestra>(entity =>
            {
                entity.ToTable("muestra");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_muestra");
                entity.Property(e => e.CosechaId).HasColumnName("id_cosecha");
                entity.Property(e => e.FechaMuestreo).HasColumnName("fecha_muestreo");
            });

            modelBuilder.Entity<MuestraTipo>(entity =>
            {
                entity.ToTable("muestra_tipo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_muestra_tipo");
                entity.Property(e => e.MuestraId).HasColumnName("id_muestra");
                entity.Property(e => e.Tipo).HasColumnName("tipo").HasConversion<string>();
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            });

            modelBuilder.Entity<BayaDiametro>(entity =>
            {
                entity.ToTable("baya_diametro");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_baya_diametro");
                entity.Property(e => e.MuestraId).HasColumnName("id_muestra");
                entity.Property(e => e.NumeroBaya).HasColumnName("numero_baya");
                entity.Property(e => e.Diametro).HasColumnName("diametro");
            });

            modelBuilder.Entity<BayaPeso>(entity =>
            {
                entity.ToTable("baya_peso");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_baya_peso");
                entity.Property(e => e.MuestraId).HasColumnName("id_muestra");
                entity.Property(e => e.NumeroBaya).HasColumnName("numero_baya");
                entity.Property(e => e.Peso).HasColumnName("peso");
            });

            modelBuilder.Entity<BayaBrix>(entity =>
            {
                entity.ToTable("baya_brix");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_baya_brix");
                entity.Property(e => e.MuestraId).HasColumnName("id_muestra");
                entity.Property(e => e.NumeroBaya).HasColumnName("numero_baya");
                entity.Property(e => e.Brix).HasColumnName("brix");
            });

            // Domain agricultural entities - explicit configurations
            modelBuilder.ApplyConfiguration(new VariedadConfiguration());
            modelBuilder.ApplyConfiguration(new LoteConfiguration());
            modelBuilder.ApplyConfiguration(new CosechaConfiguration());
            modelBuilder.ApplyConfiguration(new BayaBrixConfiguration());
        }
    }
}
