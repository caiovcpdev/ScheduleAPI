using Microsoft.AspNetCore.Mvc;
using ScheduleAPI.Application.DTOs.Agendamento;
using ScheduleAPI.Application.Interfaces;

namespace ScheduleAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        private readonly IAgendamentoSerivce _service;
        public AgendamentosController(IAgendamentoSerivce service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var agendamentos = await _service.ObterTodosAsync();
            return agendamentos is null ? NotFound() : Ok(agendamentos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var agendamento = await _service.ObterPorIdAsync(id);
            return agendamento is null ? NotFound() : Ok(agendamento);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] AgendamentoRequestDto dto)
        {
            var agendamento = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = agendamento.Id }, agendamento);
        }

        [HttpPatch("{id:guid}/confirmar")]
        public async Task<IActionResult> Confirmar(Guid id)
        {
            await _service.ConfirmarAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:guid}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            await _service.CancelarAsync(id);
            return NoContent();
        }

    }   
}
