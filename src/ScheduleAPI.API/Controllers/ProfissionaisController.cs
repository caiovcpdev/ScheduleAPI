using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.Interfaces;

namespace ScheduleAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfissionaisController : ControllerBase
    {
        private readonly IProfissionalService _service;
        public ProfissionaisController(IProfissionalService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var profissionais = await _service.ObterTodosAsync();
            return Ok(profissionais);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var profissional = await _service.ObterPorIdAsync(id);
            return Ok(profissional);
        }

        [HttpGet("{id:guid}/servicos")]
        public async Task<IActionResult> ObterServicoPorProfissional(Guid id)
        {
            var servicos = await _service.ObterServicoPorProfissionalAsync(id);
            return Ok(servicos);
        }

        [HttpGet("{id:guid}/disponibilidade")]
        public async Task<IActionResult> ObterDisponibilidade(Guid id, [FromQuery] DateTime data, [FromQuery] int intervalo = 30)
        {
            var disponibilidade = await _service.ObterDisponibilidadeAsync(id, data, intervalo);
            return Ok(disponibilidade);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] //Apenas usuários com a role "Admin" podem criar profissionais
        public async Task<IActionResult> Criar([FromBody] ProfissionalRequestDto dto)
        {
            var profissional = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = profissional.Id}, profissional);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")] //Apenas usuários com a role "Admin" podem atualizar profissionais
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProfissionalRequestDto dto)
        {
            var profissional = await _service.AtualizarAsync(id, dto);
            return Ok(profissional);
        }

        [HttpPost("{id:guid}/servicos/{servicoId:guid}")]
        [Authorize(Roles = "Admin")] // só Admin vincula serviço
        public async Task<IActionResult> VincularServico(Guid id, Guid servicoId)
        {
            await _service.VincularServicoAsync(id, servicoId);
            return NoContent();
        }
    }
}
