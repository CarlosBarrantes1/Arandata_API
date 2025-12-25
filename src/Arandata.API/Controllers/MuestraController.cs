using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Arandata.API.Controllers
{
    public class CreateMuestraRequest
    {
        public int id_cosecha { get; set; }
        public string fecha_muestreo { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class MuestraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MuestraController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var muestras = _context.Muestras
                .Select(m => new
                {
                    id_muestra = m.Id,
                    id_cosecha = m.CosechaId,
                    fecha_muestreo = m.FechaMuestreo.ToString("yyyy-MM-dd")
                })
                .ToList();
            return Ok(muestras);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var muestra = _context.Muestras
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    id_muestra = m.Id,
                    id_cosecha = m.CosechaId,
                    fecha_muestreo = m.FechaMuestreo.ToString("yyyy-MM-dd")
                })
                .FirstOrDefault();
            if (muestra == null) return NotFound();
            return Ok(muestra);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMuestraRequest dto)
        {
            try
            {
                if (dto == null) return BadRequest(new { error = "El cuerpo de la solicitud es nulo." });

                // Verificar si la cosecha existe
                var cosechaExiste = _context.Cosechas.Any(c => c.Id == dto.id_cosecha);
                if (!cosechaExiste)
                {
                    return BadRequest(new { error = $"La cosecha con ID {dto.id_cosecha} no existe." });
                }

                if (!System.DateTime.TryParse(dto.fecha_muestreo, out var fechaParsed))
                {
                    return BadRequest(new { error = "Formato de fecha inválido. Use YYYY-MM-DD." });
                }

                var muestra = new Muestra
                {
                    CosechaId = dto.id_cosecha,
                    FechaMuestreo = fechaParsed
                };

                _context.Muestras.Add(muestra);
                await _context.SaveChangesAsync();

                // AGREGADO: Crear configuraciones por defecto para la muestra (Senior Logic)
                var tiposDefault = new List<MuestraTipo>
                {
                    new MuestraTipo { MuestraId = muestra.Id, Tipo = TipoMuestra.BRIX, Cantidad = 12 },
                    new MuestraTipo { MuestraId = muestra.Id, Tipo = TipoMuestra.DIAMETRO, Cantidad = 100 },
                    new MuestraTipo { MuestraId = muestra.Id, Tipo = TipoMuestra.PESO, Cantidad = 20 }
                };
                _context.MuestraTipos.AddRange(tiposDefault);
                await _context.SaveChangesAsync();

                var respuesta = new
                {
                    id_muestra = muestra.Id,
                    id_cosecha = muestra.CosechaId,
                    fecha_muestreo = muestra.FechaMuestreo.ToString("yyyy-MM-dd")
                };
                return Created(string.Empty, respuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR AL GUARDAR MUESTRA: " + ex.ToString());
                return StatusCode(500, new { error = "Error interno al guardar la muestra.", detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] object dto)
        {
            // Lógica para actualizar una muestra
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var muestra = await _context.Muestras.FindAsync(id);
            if (muestra == null) return NotFound();

            // Lógica Senior: Eliminar en cascada manual para asegurar limpieza
            var tipos = _context.MuestraTipos.Where(t => t.MuestraId == id);
            var brix = _context.BayasBrix.Where(b => b.MuestraId == id);
            var diametros = _context.BayasDiametro.Where(b => b.MuestraId == id);
            var pesos = _context.BayasPeso.Where(b => b.MuestraId == id);

            _context.MuestraTipos.RemoveRange(tipos);
            _context.BayasBrix.RemoveRange(brix);
            _context.BayasDiametro.RemoveRange(diametros);
            _context.BayasPeso.RemoveRange(pesos);
            _context.Muestras.Remove(muestra);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
