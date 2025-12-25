using Microsoft.AspNetCore.Mvc;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Arandata.API.Controllers
{
    public class CreateBayaDiametroRequest
    {
        public int id_muestra { get; set; }
        public int? numero_baya { get; set; }
        public decimal diametro { get; set; }
    }

    public class BulkBayaDiametroRequest
    {
        public int id_muestra { get; set; }
        public List<decimal>? valores { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class BayaDiametroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BayaDiametroController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBayaDiametroRequest dto)
        {
            var config = await _context.MuestraTipos
                .FirstOrDefaultAsync(t => t.MuestraId == dto.id_muestra && t.Tipo == TipoMuestra.DIAMETRO);

            if (config == null) return BadRequest(new { error = "No se ha configurado DIAMETRO para esta muestra." });

            var conteoActual = await _context.BayasDiametro.CountAsync(b => b.MuestraId == dto.id_muestra);

            if (conteoActual >= config.Cantidad)
                return BadRequest(new { error = "Límite excedido", message = $"Ya se registraron las {config.Cantidad} bayas." });

            var baya = new BayaDiametro
            {
                MuestraId = dto.id_muestra,
                NumeroBaya = dto.numero_baya ?? (conteoActual + 1),
                Diametro = dto.diametro
            };

            _context.BayasDiametro.Add(baya);
            await _context.SaveChangesAsync();
            return Created("", baya);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] BulkBayaDiametroRequest dto)
        {
            if (dto == null || dto.valores == null || !dto.valores.Any())
                return BadRequest(new { error = "Debe enviar una lista de valores." });

            var config = await _context.MuestraTipos
                .FirstOrDefaultAsync(t => t.MuestraId == dto.id_muestra && t.Tipo == TipoMuestra.DIAMETRO);

            if (config == null) return BadRequest(new { error = "No se ha configurado DIAMETRO para esta muestra." });

            var conteoActual = await _context.BayasDiametro.CountAsync(b => b.MuestraId == dto.id_muestra);
            var espacioDisponible = config.Cantidad - conteoActual;

            if (dto.valores.Count > espacioDisponible)
                return BadRequest(new { error = "Exceso de datos", message = $"Solo quedan {espacioDisponible} espacios disponibles." });

            var nuevasBayas = new List<BayaDiametro>();
            for (int i = 0; i < dto.valores.Count; i++)
            {
                nuevasBayas.Add(new BayaDiametro
                {
                    MuestraId = dto.id_muestra,
                    NumeroBaya = conteoActual + i + 1,
                    Diametro = dto.valores[i]
                });
            }

            _context.BayasDiametro.AddRange(nuevasBayas);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{nuevasBayas.Count} registros de Diámetro guardados.", total = conteoActual + nuevasBayas.Count });
        }

        [HttpGet("muestra/{muestraId}")]
        public async Task<IActionResult> GetByMuestra(int muestraId)
        {
            var bayas = await _context.BayasDiametro
                .Where(b => b.MuestraId == muestraId)
                .ToListAsync();
            return Ok(bayas);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.BayasDiametro.FindAsync(id);
            if (item == null) return NotFound();
            _context.BayasDiametro.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
