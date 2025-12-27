using Microsoft.AspNetCore.Mvc;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Arandata.API.Controllers
{
    public class CreateBayaPesoRequest
    {
        public int id_muestra { get; set; }
        public int? numero_baya { get; set; }
        public decimal peso { get; set; }
    }

    public class BulkBayaPesoRequest
    {
        public int id_muestra { get; set; }
        public List<decimal>? valores { get; set; }
    }

    [ApiController]
    [Route("api/pesos")]
    public class BayaPesoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BayaPesoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("muestras/{id_muestra}/pesos")]
        public async Task<IActionResult> GetByMuestra(int id_muestra)
        {
            var bayas = await _context.BayasPeso
                .Where(b => b.MuestraId == id_muestra)
                .OrderBy(b => b.NumeroBaya)
                .ToListAsync();
            return Ok(bayas);
        }

        [HttpPost("muestras/{id_muestra}/pesos")]
        public async Task<IActionResult> CreateForMuestra(int id_muestra, [FromBody] CreateBayaPesoRequest dto)
        {
            dto.id_muestra = id_muestra;
            var config = await _context.MuestraTipos
                .FirstOrDefaultAsync(t => t.MuestraId == dto.id_muestra && t.Tipo == TipoMuestra.PESO);

            if (config == null) return BadRequest(new { error = "No se ha configurado PESO para esta muestra." });

            var conteoActual = await _context.BayasPeso.CountAsync(b => b.MuestraId == dto.id_muestra);

            if (conteoActual >= config.Cantidad)
                return BadRequest(new { error = "Límite excedido", message = $"Ya se registraron las {config.Cantidad} bayas." });

            var baya = new BayaPeso
            {
                MuestraId = dto.id_muestra,
                NumeroBaya = dto.numero_baya ?? (conteoActual + 1),
                Peso = dto.peso
            };

            _context.BayasPeso.Add(baya);
            await _context.SaveChangesAsync();
            return Created("", baya);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] BulkBayaPesoRequest dto)
        {
            if (dto == null || dto.valores == null || !dto.valores.Any())
                return BadRequest(new { error = "Debe enviar una lista de valores." });

            var config = await _context.MuestraTipos
                .FirstOrDefaultAsync(t => t.MuestraId == dto.id_muestra && t.Tipo == TipoMuestra.PESO);

            if (config == null) return BadRequest(new { error = "No se ha configurado PESO para esta muestra." });

            var conteoActual = await _context.BayasPeso.CountAsync(b => b.MuestraId == dto.id_muestra);
            var espacioDisponible = config.Cantidad - conteoActual;

            if (dto.valores.Count > espacioDisponible)
                return BadRequest(new { error = "Exceso de datos", message = $"Solo quedan {espacioDisponible} espacios disponibles." });

            var nuevasBayas = new List<BayaPeso>();
            for (int i = 0; i < dto.valores.Count; i++)
            {
                nuevasBayas.Add(new BayaPeso
                {
                    MuestraId = dto.id_muestra,
                    NumeroBaya = conteoActual + i + 1,
                    Peso = dto.valores[i]
                });
            }

            _context.BayasPeso.AddRange(nuevasBayas);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{nuevasBayas.Count} registros de Peso guardados.", total = conteoActual + nuevasBayas.Count });
        }

        [HttpDelete("pesos/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.BayasPeso.FindAsync(id);
            if (item == null) return NotFound();
            _context.BayasPeso.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
