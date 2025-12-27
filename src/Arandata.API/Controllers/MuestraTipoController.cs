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
    [Route("api/muestras")]
    public class MuestraTipoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MuestraTipoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id_muestra}/tipos")]
        public async Task<IActionResult> GetTipos(int id_muestra)
        {
            return Ok(await _context.MuestraTipos.Where(t => t.MuestraId == id_muestra).ToListAsync());
        }

        [HttpPost("{id_muestra}/tipos")]
        public async Task<IActionResult> SetTipos(int id_muestra, [FromBody] List<MuestraTipo> tipos)
        {
            var existentes = _context.MuestraTipos.Where(t => t.MuestraId == id_muestra);
            _context.MuestraTipos.RemoveRange(existentes);

            foreach (var t in tipos) { t.MuestraId = id_muestra; }
            _context.MuestraTipos.AddRange(tipos);
            await _context.SaveChangesAsync();
            return Ok(tipos);
        }
    }
}
