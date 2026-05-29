using Microsoft.AspNetCore.Mvc;
using ScheduleAPI.Application.DTOs.Servico;
using ScheduleAPI.Application.Interfaces;

namespace ScheduleAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicosController : ControllerBase
    {
        private readonly IServicoService _service;
        public ServicosController(IServicoService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var servicos = await _service.ObterTodosAsync();
            return servicos is null ? NotFound() : Ok(servicos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var servico = await _service.ObterPorIdAsync(id);
            return servico is null ? NotFound() : Ok(servico);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ServicoRequestDto dto)
        {
            var servico = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = servico.Id}, servico);
        }
    }
}
