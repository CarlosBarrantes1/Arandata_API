using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Arandata.API.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            var method = context.Request.Method.ToUpper();

            // 1. Excluir rutas públicas (Login, Swagger, Seed)
            if (path.Contains("/auth/") || path.Contains("/swagger") || path.Contains("/seed-security"))
            {
                await _next(context);
                return;
            }

            // 2. Obtener el UsuarioId desde los headers (Simulación de JWT para este prompt)
            if (!context.Request.Headers.TryGetValue("X-User-Id", out var userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                // Si no hay usuario, permitimos que siga pero el controlador podría fallar si requiere auth.
                // En un sistema real, aquí devolveríamos 401 Unauthorized.
                await _next(context);
                return;
            }

            // 3. Identificar el Módulo basado en la ruta
            string moduloCodigo = "";
            if (path.Contains("/lote") || path.Contains("/variedad")) moduloCodigo = "LOT";
            else if (path.Contains("/cosecha") || path.Contains("/poda")) moduloCodigo = "COS";
            else if (path.Contains("/muestra") || path.Contains("/baya")) moduloCodigo = "MUE";
            else if (path.Contains("/report")) moduloCodigo = "REP";
            else if (path.Contains("/security")) moduloCodigo = "ADM";

            if (string.IsNullOrEmpty(moduloCodigo))
            {
                await _next(context);
                return;
            }

            // 4. Verificar permisos en la base de datos
            var permisos = await dbContext.UsuarioRoles
                .Where(ur => ur.UsuarioId == userId)
                .SelectMany(ur => ur.Rol!.RolModulos)
                .Where(rm => rm.Modulo!.Codigo == moduloCodigo)
                .ToListAsync();

            bool tienePermiso = false;

            if (method == "GET") tienePermiso = permisos.Any(p => p.PuedeVer);
            else if (method == "POST") tienePermiso = permisos.Any(p => p.PuedeCrear);
            else if (method == "PUT" || method == "PATCH") tienePermiso = permisos.Any(p => p.PuedeEditar);
            else if (method == "DELETE") tienePermiso = permisos.Any(p => p.PuedeEliminar);

            if (!tienePermiso)
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Acceso Denegado",
                    mensaje = $"No tienes permisos para realizar esta acción ({method}) en el módulo {moduloCodigo}."
                });
                return;
            }

            await _next(context);
        }
    }
}
