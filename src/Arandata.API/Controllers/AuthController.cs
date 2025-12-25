using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Arandata.API.Controllers
{
    public class LoginRequest { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                        .ThenInclude(r => r!.RolModulos)
                            .ThenInclude(rm => rm.Modulo)
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Activo);

            if (usuario == null) return Unauthorized(new { error = "Usuario no encontrado o inactivo" });

            // NOTA: En producción usar BCrypt para verificar el hash. Aquí simplificamos para el prompt.
            if (usuario.PasswordHash != request.Password) return Unauthorized(new { error = "Contraseña incorrecta" });

            // Construir el mapa de permisos para el frontend
            var permisos = usuario.UsuarioRoles
                .SelectMany(ur => ur.Rol!.RolModulos)
                .GroupBy(rm => rm.Modulo!.Codigo)
                .Select(g => new
                {
                    modulo = g.Key,
                    ver = g.Any(x => x.PuedeVer),
                    crear = g.Any(x => x.PuedeCrear),
                    editar = g.Any(x => x.PuedeEditar),
                    eliminar = g.Any(x => x.PuedeEliminar)
                });

            return Ok(new
            {
                id = usuario.Id,
                username = usuario.Username,
                roles = usuario.UsuarioRoles.Select(ur => ur.Rol!.Nombre),
                permisos = permisos
            });
        }
    }
}
