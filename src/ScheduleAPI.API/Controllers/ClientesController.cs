using Microsoft.AspNetCore.Mvc;
using ScheduleAPI.Application.DTOs.Cliente;
using ScheduleAPI.Application.Interfaces;

namespace ScheduleAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteSerivce _service;
        public ClientesController(IClienteSerivce service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var clientes = await _service.ObterTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cliente = await _service.ObterPorIdAsync(id);
            return Ok(cliente);
        }

        [HttpGet("por-email")]
        public async Task<IActionResult> ObterPorEmailAsync([FromQuery] string email)
        {
            var cliente = await _service.ObterPorEmailAsync(email);
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ClienteRequestDto dto)
        {
            var cliente = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id}, cliente);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ClienteRequestDto dto)
        {
            var cliente = await _service.AtualizarAsync(id, dto);
            return Ok(cliente);
        }
    }
}
