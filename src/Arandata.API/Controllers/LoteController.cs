using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Lote;
using System.Threading.Tasks;
using Arandata.Infrastructure.Persistence.Context;
using Arandata.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoteController : ControllerBase
    {
        private readonly ILoteService _service;
        private readonly ApplicationDbContext _context;

        public LoteController(ILoteService service, ApplicationDbContext context)
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateLoteDto dto)
        {
            var created = await _service.CreateAsync(dto);

            // Lógica Senior: Si el lote tiene fecha de poda inicial, registrarla en la tabla Podas automáticamente
            if (dto.FechaPoda.HasValue)
            {
                var podaExistente = await _context.Podas.AnyAsync(p => p.LoteId == created.idLote && p.FechaPoda == dto.FechaPoda.Value);
                if (!podaExistente)
                {
                    _context.Podas.Add(new Poda { LoteId = created.idLote, FechaPoda = dto.FechaPoda.Value });
                    await _context.SaveChangesAsync();
                }
            }

            return CreatedAtAction(nameof(Get), new { id = created.idLote }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateLoteDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
