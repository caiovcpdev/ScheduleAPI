using ScheduleAPI.Application.DTOs.Cliente;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Interfaces;

namespace ScheduleAPI.Application.Service
{
    public class ClienteService : IClienteSerivce
    {
        private readonly IClienteRepository _repository;
        public ClienteService(IClienteRepository repository) => _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<ClienteResponseDto> AtualizarAsync(Guid id, ClienteRequestDto dto)
        {
            var cliente = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"Cliente com id {id} não encontrado.");

            cliente.Atualizar(dto.Nome, dto.Email, dto.Telefone);
            await _repository.AtualizarAsync(cliente);
            return ToDto(cliente);
        }

        public async Task<ClienteResponseDto> CriarAsync(ClienteRequestDto dto)
        {
            var cliente = new Cliente(dto.Nome, dto.Email, dto.Telefone);
            await _repository.AdicionarAsync(cliente);
            return ToDto(cliente);
        }


        public async Task<ClienteResponseDto?> ObterPorIdAsync(Guid id)
        {
            var cliente  = await _repository.ObterPorIdAsync(id);
            return cliente is null ? null : ToDto(cliente);
        }

        public async Task<IEnumerable<ClienteResponseDto>> ObterTodosAsync()
        {
            var clientes = await _repository.ObterTodosAsync();
            return clientes.Select(ToDto);
        }

        // Metodo auxiliar para converter Cliente para ClienteResponseDto
        private static ClienteResponseDto ToDto(Cliente c) => new(c.Id, c.Nome, c.Email, c.Telefone, c.CreatedAt);
    }
}
