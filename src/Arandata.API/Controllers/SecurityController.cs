using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SecurityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios() => Ok(await _context.Usuarios.Include(u => u.UsuarioRoles).ThenInclude(ur => ur.Rol).ToListAsync());

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles() => Ok(await _context.Roles.Include(r => r.RolModulos).ThenInclude(rm => rm.Modulo).ToListAsync());

        [HttpPost("seed-security")]
        public async Task<IActionResult> SeedSecurity()
        {
            if (await _context.Usuarios.AnyAsync()) return BadRequest("La seguridad ya ha sido inicializada.");

            // 1. Crear Módulos
            var modulos = new List<Modulo>
            {
                new Modulo { Nombre = "Lotes y Variedades", Codigo = "LOT" },
                new Modulo { Nombre = "Cosechas y Podas", Codigo = "COS" },
                new Modulo { Nombre = "Muestreos de Calidad", Codigo = "MUE" },
                new Modulo { Nombre = "Reportes Estratégicos", Codigo = "REP" },
                new Modulo { Nombre = "Administración de Sistema", Codigo = "ADM" }
            };
            _context.Modulos.AddRange(modulos);
            await _context.SaveChangesAsync();

            // 2. Crear Roles
            var rolAdmin = new Rol { Nombre = "Administrador", Descripcion = "Gestión total" };
            var rolIngeniero = new Rol { Nombre = "Ingeniero Agrónomo", Descripcion = "Acceso total a registros y reportes" };
            var rolOperador = new Rol { Nombre = "Operador de Campo", Descripcion = "Registra datos de campo" };
            var rolGerente = new Rol { Nombre = "Gerente", Descripcion = "Solo lectura de reportes" };

            _context.Roles.AddRange(rolAdmin, rolIngeniero, rolOperador, rolGerente);
            await _context.SaveChangesAsync();

            // 3. Asignar Permisos a Roles
            // Admin: Todo
            foreach (var m in modulos)
            {
                _context.RolModulos.Add(new RolModulo { RolId = rolAdmin.Id, ModuloId = m.Id, PuedeVer = true, PuedeCrear = true, PuedeEditar = true, PuedeEliminar = true });
            }

            // Operador: Solo ver y crear en Lotes, Cosechas y Muestreos. Ver reportes.
            var modulosOperador = modulos.Where(m => m.Codigo != "ADM").ToList();
            foreach (var m in modulosOperador)
            {
                _context.RolModulos.Add(new RolModulo
                {
                    RolId = rolOperador.Id,
                    ModuloId = m.Id,
                    PuedeVer = true,
                    PuedeCrear = (m.Codigo != "REP"),
                    PuedeEditar = false,
                    PuedeEliminar = false
                });
            }

            // Gerente: Solo ver Reportes
            var modRep = modulos.First(m => m.Codigo == "REP");
            _context.RolModulos.Add(new RolModulo { RolId = rolGerente.Id, ModuloId = modRep.Id, PuedeVer = true, PuedeCrear = false, PuedeEditar = false, PuedeEliminar = false });

            await _context.SaveChangesAsync();

            // 4. Crear Usuario Inicial
            var adminUser = new Usuario { Username = "admin", PasswordHash = "admin123", Email = "admin@arandata.com" };
            _context.Usuarios.Add(adminUser);
            await _context.SaveChangesAsync();

            _context.UsuarioRoles.Add(new UsuarioRol { UsuarioId = adminUser.Id, RolId = rolAdmin.Id });
            await _context.SaveChangesAsync();

            return Ok("Módulo de seguridad inicializado con éxito.");
        }
    }
}
