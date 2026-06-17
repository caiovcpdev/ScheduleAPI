using ScheduleAPI.Application.DTOs.Cliente;
using ScheduleAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Interfaces
{
    public interface IClienteSerivce
    {
        Task<IEnumerable<ClienteResponseDto>> ObterTodosAsync();
        Task<ClienteResponseDto?> ObterPorIdAsync(Guid id);
        Task<ClienteResponseDto?> ObterPorEmailAsync(string email);
        Task<ClienteResponseDto> CriarAsync(ClienteRequestDto dto);
        Task<ClienteResponseDto> AtualizarAsync(Guid id, ClienteRequestDto dto);
    }
}
