using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.BayaBrix;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Arandata.API.Controllers
{
    public class CreateBayaBrixRequest
    {
        public int id_muestra { get; set; }
        public int? numero_baya { get; set; } // Opcional ahora
        public decimal brix { get; set; }
    }

    public class BulkBayaBrixRequest
    {
        public int id_muestra { get; set; }
        public List<decimal>? valores { get; set; } // Marcado como nullable para evitar advertencias de compilación
    }

    [ApiController]
    [Route("api/brix")]
    public class BayaBrixController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BayaBrixController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("muestras/{id_muestra}/brix")]
        public async Task<IActionResult> CreateForMuestra(int id_muestra, [FromBody] CreateBayaBrixRequest dto)
        {
            dto.id_muestra = id_muestra;
            return await Create(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBayaBrixRequest dto)
        {
            var config = await _context.MuestraTipos
                .FirstOrDefaultAsync(t => t.MuestraId == dto.id_muestra && t.Tipo == TipoMuestra.BRIX);

            if (config == null) return BadRequest(new { error = "No se ha configurado BRIX para esta muestra." });

            var conteoActual = await _context.BayasBrix.CountAsync(b => b.MuestraId == dto.id_muestra);

            if (conteoActual >= config.Cantidad)
                return BadRequest(new { error = "Límite excedido", message = $"Ya se registraron las {config.Cantidad} bayas." });

            var baya = new BayaBrix
            {
                MuestraId = dto.id_muestra,
                NumeroBaya = dto.numero_baya ?? (conteoActual + 1), // Auto-numeración
                Brix = dto.brix
            };

            _context.BayasBrix.Add(baya);
            await _context.SaveChangesAsync();
            return Created("", baya);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] BulkBayaBrixRequest dto)
        {
            if (dto == null || dto.valores == null || !dto.valores.Any())
                return BadRequest(new { error = "Debe enviar una lista de valores." });

            var config = await _context.MuestraTipos
                .FirstOrDefaultAsync(t => t.MuestraId == dto.id_muestra && t.Tipo == TipoMuestra.BRIX);

            if (config == null) return BadRequest(new { error = "No se ha configurado BRIX para esta muestra." });

            var conteoActual = await _context.BayasBrix.CountAsync(b => b.MuestraId == dto.id_muestra);
            var espacioDisponible = config.Cantidad - conteoActual;

            if (dto.valores.Count > espacioDisponible)
                return BadRequest(new { error = "Exceso de datos", message = $"Solo quedan {espacioDisponible} espacios disponibles." });

            var nuevasBayas = new List<BayaBrix>();
            for (int i = 0; i < dto.valores.Count; i++)
            {
                nuevasBayas.Add(new BayaBrix
                {
                    MuestraId = dto.id_muestra,
                    NumeroBaya = conteoActual + i + 1,
                    Brix = dto.valores[i]
                });
            }

            _context.BayasBrix.AddRange(nuevasBayas);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{nuevasBayas.Count} registros de Brix guardados.", total = conteoActual + nuevasBayas.Count });
        }

        [HttpGet("muestras/{id_muestra}/brix")]
        public async Task<IActionResult> GetByMuestra(int id_muestra)
        {
            var bayas = await _context.BayasBrix.Where(b => b.MuestraId == id_muestra).ToListAsync();
            return Ok(bayas);
        }

        [HttpPut("brix/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBayaBrixRequest dto)
        {
            var item = await _context.BayasBrix.FindAsync(id);
            if (item == null) return NotFound();
            item.Brix = dto.brix;
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("brix/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.BayasBrix.FindAsync(id);
            if (item == null) return NotFound();
            _context.BayasBrix.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
