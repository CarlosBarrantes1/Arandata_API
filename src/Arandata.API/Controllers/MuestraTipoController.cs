using Microsoft.AspNetCore.Mvc;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Arandata.API.Controllers
{
    public class CreateMuestraTipoRequest
    {
        public int id_muestra { get; set; }
        public string tipo { get; set; } // DIAMETRO, PESO, BRIX
        public int cantidad { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MuestraTipoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MuestraTipoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMuestraTipoRequest dto)
        {
            if (!System.Enum.TryParse<TipoMuestra>(dto.tipo, true, out var tipoEnum))
            {
                return BadRequest(new { error = "Tipo de muestra inválido. Use DIAMETRO, PESO o BRIX." });
            }

            var muestraTipo = new MuestraTipo
            {
                MuestraId = dto.id_muestra,
                Tipo = tipoEnum,
                Cantidad = dto.cantidad
            };

            _context.MuestraTipos.Add(muestraTipo);
            await _context.SaveChangesAsync();

            return Created("", new
            {
                id_muestra_tipo = muestraTipo.Id,
                id_muestra = muestraTipo.MuestraId,
                tipo = muestraTipo.Tipo.ToString(),
                cantidad = muestraTipo.Cantidad
            });
        }

        [HttpGet("muestra/{muestraId}")]
        public async Task<IActionResult> GetByMuestra(int muestraId)
        {
            var tipos = await _context.MuestraTipos
                .Where(t => t.MuestraId == muestraId)
                .Select(t => new
                {
                    id_muestra_tipo = t.Id,
                    tipo = t.Tipo.ToString(),
                    cantidad = t.Cantidad
                })
                .ToListAsync();
            return Ok(tipos);
        }
    }
}
