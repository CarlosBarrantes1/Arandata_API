using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Reporte 1: Dashboard de Producción por Lote
        /// Proporciona una visión general de la eficiencia de cada lote.
        /// </summary>
        [HttpGet("produccion-lotes")]
        public async Task<IActionResult> GetProduccionLotes()
        {
            var reporte = await _context.Lotes
                .Select(l => new
                {
                    loteId = l.Id,
                    nombreLote = l.Nombre,
                    variedad = l.Variedad != null ? l.Variedad.Nombre : "N/A",
                    totalKgCosechados = l.Cosechas.Sum(c => c.KgTotal ?? 0),
                    promedioKgPlanta = l.Cosechas.Any() ? l.Cosechas.Average(c => c.KgPlanta ?? 0) : 0,
                    ultimaCosecha = l.Cosechas.OrderByDescending(c => c.FechaCosecha).Select(c => c.FechaCosecha).FirstOrDefault(),
                    diasDesdeUltimaPoda = l.Podas.OrderByDescending(p => p.FechaPoda).Select(p => (DateTime.Now - p.FechaPoda).Days).FirstOrDefault()
                })
                .ToListAsync();

            return Ok(reporte);
        }

        /// <summary>
        /// Reporte 2: Análisis de Calidad por Muestra
        /// Calcula promedios de Brix, Diámetro y Peso para cada muestra.
        /// </summary>
        [HttpGet("calidad-muestras")]
        public async Task<IActionResult> GetCalidadMuestras()
        {
            var reporte = await _context.Muestras
                .Select(m => new
                {
                    muestraId = m.Id,
                    fechaMuestreo = m.FechaMuestreo.ToString("yyyy-MM-dd"),
                    cosechaId = m.CosechaId,
                    lote = m.Cosecha != null && m.Cosecha.Lote != null ? m.Cosecha.Lote.Nombre : "N/A",
                    brix = new
                    {
                        promedio = m.BayasBrix.Any() ? m.BayasBrix.Average(b => b.Brix) : 0,
                        conteo = m.BayasBrix.Count(),
                        max = m.BayasBrix.Any() ? m.BayasBrix.Max(b => b.Brix) : 0
                    },
                    diametro = new
                    {
                        promedio = m.BayasDiametro.Any() ? m.BayasDiametro.Average(d => d.Diametro) : 0,
                        conteo = m.BayasDiametro.Count(),
                        min = m.BayasDiametro.Any() ? m.BayasDiametro.Min(d => d.Diametro) : 0
                    },
                    peso = new
                    {
                        promedio = m.BayasPeso.Any() ? m.BayasPeso.Average(p => p.Peso) : 0,
                        conteo = m.BayasPeso.Count()
                    }
                })
                .ToListAsync();

            return Ok(reporte);
        }

        /// <summary>
        /// Reporte 3: Comparativo de Lotes (Ranking de Productividad)
        /// Dado que hay una sola variedad, comparamos el rendimiento entre lotes.
        /// </summary>
        [HttpGet("ranking-lotes")]
        public async Task<IActionResult> GetRankingLotes()
        {
            var reporte = await _context.Lotes
                .Select(l => new
                {
                    loteId = l.Id,
                    nombre = l.Nombre,
                    totalKg = l.Cosechas.Sum(c => c.KgTotal ?? 0),
                    kgPorPlanta = l.Cosechas.Any() ? l.Cosechas.Average(c => c.KgPlanta ?? 0) : 0,
                    eficiencia = l.Hectareas.HasValue && l.Hectareas > 0
                        ? l.Cosechas.Sum(c => c.KgTotal ?? 0) / l.Hectareas
                        : 0
                })
                .OrderByDescending(r => r.totalKg)
                .ToListAsync();

            return Ok(reporte);
        }

        /// <summary>
        /// Reporte 5: Evolución de Calidad Temporal
        /// Muestra cómo cambia el Brix y Diámetro a lo largo del tiempo para la variedad principal.
        /// </summary>
        [HttpGet("evolucion-calidad")]
        public async Task<IActionResult> GetEvolucionCalidad()
        {
            var reporte = await _context.Muestras
                .OrderBy(m => m.FechaMuestreo)
                .Select(m => new
                {
                    fecha = m.FechaMuestreo.ToString("yyyy-MM-dd"),
                    lote = m.Cosecha != null && m.Cosecha.Lote != null ? m.Cosecha.Lote.Nombre : "N/A",
                    brixPromedio = m.BayasBrix.Any() ? m.BayasBrix.Average(b => b.Brix) : 0,
                    diametroPromedio = m.BayasDiametro.Any() ? m.BayasDiametro.Average(d => d.Diametro) : 0
                })
                .ToListAsync();

            return Ok(reporte);
        }

        /// <summary>
        /// Reporte 4: Alertas de Muestreo Incompleto
        /// Identifica muestras que no han llegado al límite configurado.
        /// </summary>
        [HttpGet("alertas-muestreo")]
        public async Task<IActionResult> GetAlertasMuestreo()
        {
            var muestras = await _context.Muestras
                .Include(m => m.MuestraTipos)
                .Include(m => m.BayasBrix)
                .Include(m => m.BayasDiametro)
                .Include(m => m.BayasPeso)
                .ToListAsync();

            var alertas = muestras.Select(m => new
            {
                muestraId = m.Id,
                fecha = m.FechaMuestreo.ToString("yyyy-MM-dd"),
                faltantes = m.MuestraTipos.Select(t => new
                {
                    tipo = t.Tipo.ToString(),
                    configurado = t.Cantidad,
                    actual = t.Tipo == TipoMuestra.BRIX ? m.BayasBrix.Count() :
                             t.Tipo == TipoMuestra.DIAMETRO ? m.BayasDiametro.Count() :
                             m.BayasPeso.Count(),
                    completado = (t.Tipo == TipoMuestra.BRIX ? m.BayasBrix.Count() :
                                 t.Tipo == TipoMuestra.DIAMETRO ? m.BayasDiametro.Count() :
                                 m.BayasPeso.Count()) >= t.Cantidad
                }).Where(x => !x.completado).ToList()
            })
            .Where(a => a.faltantes.Any())
            .ToList();

            return Ok(alertas);
        }

        [HttpGet("lote/{idLote}")]
        public async Task<IActionResult> GetDashboardLote(int idLote)
        {
            var lote = await _context.Lotes
                .Include(l => l.Cosechas)
                .FirstOrDefaultAsync(l => l.Id == idLote);

            if (lote == null) return NotFound();

            return Ok(new
            {
                nombre = lote.Nombre,
                total_cosechado = lote.Cosechas.Sum(c => c.KgTotal),
                rendimiento_promedio = lote.Cosechas.Average(c => c.KgPlanta),
                ultima_cosecha = lote.Cosechas.OrderByDescending(c => c.FechaCosecha).FirstOrDefault()?.FechaCosecha
            });
        }

        [HttpGet("cosecha/{idCosecha}")]
        public async Task<IActionResult> GetDashboardCosecha(int idCosecha)
        {
            var cosecha = await _context.Cosechas
                .Include(c => c.Muestras)
                .FirstOrDefaultAsync(c => c.Id == idCosecha);

            if (cosecha == null) return NotFound();

            return Ok(new
            {
                fecha = cosecha.FechaCosecha,
                kg_total = cosecha.KgTotal,
                muestras_realizadas = cosecha.Muestras.Count
            });
        }
    }
}
