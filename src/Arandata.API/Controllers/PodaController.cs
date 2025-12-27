using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arandata.API.Controllers
{
    public class CreatePodaRequest
    {
        public int id_lote { get; set; }
        public string? fecha_poda { get; set; }
    }

    [ApiController]
    [Route("api/poda")] // Cambiado a singular como solicitaste
    public class PodaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PodaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var podas = await _context.Podas
                .Select(p => new
                {
                    id_poda = p.Id,
                    id_lote = p.LoteId,
                    fecha_poda = p.FechaPoda.ToString("yyyy-MM-dd")
                })
                .ToListAsync();
            return Ok(podas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var poda = await _context.Podas
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    id_poda = p.Id,
                    id_lote = p.LoteId,
                    fecha_poda = p.FechaPoda.ToString("yyyy-MM-dd")
                })
                .FirstOrDefaultAsync();

            if (poda == null) return NotFound();
            return Ok(poda);
        }

        [HttpGet("lote/{id_lote}")]
        public async Task<IActionResult> GetByLote(int id_lote)
        {
            var podas = await _context.Podas.Where(p => p.LoteId == id_lote).ToListAsync();
            return Ok(podas);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePodaRequest dto)
        {
            if (!DateTime.TryParse(dto.fecha_poda, out var fecha))
                return BadRequest(new { error = "Formato de fecha inválido." });

            var poda = new Poda
            {
                LoteId = dto.id_lote,
                FechaPoda = fecha
            };

            _context.Podas.Add(poda);
            await _context.SaveChangesAsync();

            return Created("", new
            {
                id_poda = poda.Id,
                id_lote = poda.LoteId,
                fecha_poda = poda.FechaPoda.ToString("yyyy-MM-dd")
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePodaRequest dto)
        {
            var poda = await _context.Podas.FindAsync(id);
            if (poda == null) return NotFound();
            if (DateTime.TryParse(dto.fecha_poda, out var fecha)) poda.FechaPoda = fecha;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var poda = await _context.Podas.FindAsync(id);
            if (poda == null) return NotFound();
            _context.Podas.Remove(poda);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
