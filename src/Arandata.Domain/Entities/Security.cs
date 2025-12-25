using System;
using System.Collections.Generic;

namespace Arandata.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }

    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
        public ICollection<RolModulo> RolModulos { get; set; } = new List<RolModulo>();
    }

    public class UsuarioRol
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }

    public class Modulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Ej: "Cosechas", "Lotes", "Reportes"
        public string Codigo { get; set; } = string.Empty; // Ej: "COS", "LOT", "REP"

        public ICollection<RolModulo> RolModulos { get; set; } = new List<RolModulo>();
    }

    public class RolModulo
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
        public int ModuloId { get; set; }
        public Modulo? Modulo { get; set; }

        // Permisos
        public bool PuedeVer { get; set; }
        public bool PuedeCrear { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }
    }
}
