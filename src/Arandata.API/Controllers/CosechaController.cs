using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Cosecha;
using System.Threading.Tasks;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CosechaController : ControllerBase
    {
        private readonly ICosechaService _service;

        public CosechaController(ICosechaService service)
        {
            _service = service;
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
        public async Task<IActionResult> Create(CreateCosechaDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
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
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
