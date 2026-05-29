using Microsoft.AspNetCore.Mvc;
using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.Interfaces;

namespace ScheduleAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionaisController : ControllerBase
    {
        private readonly IProfissionalService _service;
        public ProfissionaisController(IProfissionalService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var profissionais = await _service.ObterTodosAsync();
            return profissionais is null ? NotFound() : Ok(profissionais);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var profissional = await _service.ObterPorIdAsync(id);
            return profissional is null ? NotFound() : Ok(profissional);
        }

        public async Task<IActionResult> Criar([FromBody] ProfissionalRequestDto dto)
        {
            var profissional = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = profissional.Id}, profissional);
        }
    }
}
