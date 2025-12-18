using Microsoft.AspNetCore.Mvc;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.BayaBrix;
using System.Threading.Tasks;

namespace Arandata.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BayaBrixController : ControllerBase
    {
        private readonly IBayaBrixService _service;

        public BayaBrixController(IBayaBrixService service)
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
        public async Task<IActionResult> Create(CreateBayaBrixDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.IdBayaBrix }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBayaBrixDto dto)
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
