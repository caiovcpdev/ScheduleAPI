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
            return Ok(servicos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var servico = await _service.ObterPorIdAsync(id);
            return Ok(servico);
        }

        [HttpGet("{id:guid}/profissionais")]
        public async Task<IActionResult> ObterProfissionalPorServico(Guid id)
        {
            var profissionais = await _service.ObterProfissionalPorServicoAsync(id);
            return Ok(profissionais);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ServicoRequestDto dto)
        {
            var servico = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = servico.Id}, servico);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ServicoRequestDto dto)
        {
            var servico = await _service.AtualizarAsync(id, dto);
            return Ok(servico);
        }
    }
}
