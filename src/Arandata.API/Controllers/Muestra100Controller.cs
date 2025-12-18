using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Muestra100;
using System.Threading.Tasks;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Muestra100Controller : ControllerBase
    {
        private readonly IMuestra100Service _service;

        public Muestra100Controller(IMuestra100Service service)
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
        public async Task<IActionResult> Create(CreateMuestra100Dto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.IdMuestra100 }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMuestra100Dto dto)
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
