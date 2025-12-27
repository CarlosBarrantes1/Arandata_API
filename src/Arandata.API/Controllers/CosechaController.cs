using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Cosecha;
using System.Threading.Tasks;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/cosechas")]
    public class CosechaController : ControllerBase
    {
        private readonly ICosechaService _service;
        private readonly ApplicationDbContext _context; // Suponiendo que este es el nombre de tu DbContext

        public CosechaController(ICosechaService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("lote/{id_lote}")]
        public async Task<IActionResult> GetByLote(int id_lote)
        {
            var cosechas = await _context.Cosechas
                .Where(c => c.LoteId == id_lote)
                .OrderByDescending(c => c.FechaCosecha)
                .ToListAsync();
            return Ok(cosechas);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCosechaDto dto)
        {
            try
            {
                // Verificar si el lote existe
                var loteExiste = await _context.Lotes.AnyAsync(l => l.Id == dto.LoteId);
                if (!loteExiste)
                {
                    return BadRequest(new { error = $"El lote con ID {dto.LoteId} no existe." });
                }

                // Lógica Senior: Calcular días después de la última poda
                var ultimaPoda = await _context.Podas
                    .Where(p => p.LoteId == dto.LoteId && p.FechaPoda <= dto.FechaCosecha)
                    .OrderByDescending(p => p.FechaPoda)
                    .FirstOrDefaultAsync();

                int? diasDespuesPoda = null;
                if (ultimaPoda != null)
                {
                    diasDespuesPoda = (dto.FechaCosecha - ultimaPoda.FechaPoda).Days;
                }

                var lote = await _context.Lotes.FindAsync(dto.LoteId);

                var cosecha = new Cosecha
                {
                    LoteId = dto.LoteId,
                    FechaCosecha = dto.FechaCosecha,
                    KgTotal = dto.KgTotal,
                    KgPlanta = dto.KgPlanta ?? (lote != null && lote.PlantasTotales > 0 ? (dto.KgTotal ?? 0) / lote.PlantasTotales : 0), // Auto-calculo de Kg/Planta
                    DiasDespuesPoda = diasDespuesPoda
                };

                // Calcular acumulados (Lógica de negocio)
                var acumuladoAnterior = await _context.Cosechas
                    .Where(c => c.LoteId == dto.LoteId)
                    .SumAsync(c => c.KgTotal ?? 0);

                cosecha.KgTotalAcumulado = acumuladoAnterior + (dto.KgTotal ?? 0);

                // Lógica Senior: Calcular KgPlantaAcumulado
                if (lote != null && lote.PlantasTotales > 0)
                {
                    cosecha.KgPlantaAcumulado = cosecha.KgTotalAcumulado / lote.PlantasTotales;
                }

                _context.Cosechas.Add(cosecha);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = cosecha.Id }, cosecha);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCosechaDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cosecha = await _context.Cosechas.FindAsync(id);
                if (cosecha == null) return NotFound();

                // Lógica Senior: Eliminar muestras asociadas antes de borrar la cosecha
                var muestras = _context.Muestras.Where(m => m.CosechaId == id).ToList();
                foreach (var muestra in muestras)
                {
                    // Limpiar bayas y tipos de cada muestra
                    var tipos = _context.MuestraTipos.Where(t => t.MuestraId == muestra.Id);
                    var brix = _context.BayasBrix.Where(b => b.MuestraId == muestra.Id);
                    var diametros = _context.BayasDiametro.Where(b => b.MuestraId == muestra.Id);
                    var pesos = _context.BayasPeso.Where(b => b.MuestraId == muestra.Id);

                    _context.MuestraTipos.RemoveRange(tipos);
                    _context.BayasBrix.RemoveRange(brix);
                    _context.BayasDiametro.RemoveRange(diametros);
                    _context.BayasPeso.RemoveRange(pesos);
                    _context.Muestras.Remove(muestra);
                }

                _context.Cosechas.Remove(cosecha);
                await _context.SaveChangesAsync();

                // Lógica Senior: Recalcular acumulados de las cosechas posteriores del mismo lote
                var cosechasPosteriores = await _context.Cosechas
                    .Where(c => c.LoteId == cosecha.LoteId && c.FechaCosecha > cosecha.FechaCosecha)
                    .OrderBy(c => c.FechaCosecha)
                    .ToListAsync();

                if (cosechasPosteriores.Any())
                {
                    var acumulado = await _context.Cosechas
                        .Where(c => c.LoteId == cosecha.LoteId && c.FechaCosecha <= cosecha.FechaCosecha)
                        .SumAsync(c => c.KgTotal ?? 0);

                    var lote = await _context.Lotes.FindAsync(cosecha.LoteId);

                    foreach (var cp in cosechasPosteriores)
                    {
                        acumulado += (cp.KgTotal ?? 0);
                        cp.KgTotalAcumulado = acumulado;
                        if (lote != null && lote.PlantasTotales > 0)
                        {
                            cp.KgPlantaAcumulado = cp.KgTotalAcumulado / lote.PlantasTotales;
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar cosecha y recalcular datos.", detalle = ex.Message });
            }
        }
    }
}
