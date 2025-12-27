using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Variedad;
using System.Threading.Tasks;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/variedades")]
    public class VariedadController : ControllerBase
    {
        private readonly IVariedadService _service;
        private readonly ApplicationDbContext _context;

        public VariedadController(IVariedadService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var variedades = await _service.GetAllAsync();
            return Ok(variedades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVariedadDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.IdVariedad }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateVariedadDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tieneLotes = await _context.Lotes.AnyAsync(l => l.VariedadId == id);
            if (tieneLotes)
            {
                return BadRequest(new { error = "No se puede eliminar la variedad porque tiene lotes asociados. Elimine los lotes primero." });
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
