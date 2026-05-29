using ScheduleAPI.Application.DTOs.Agendamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Interfaces
{
    public interface IAgendamentoSerivce
    {
        Task<IEnumerable<AgendamentoResponseDto>> ObterTodosAsync();
        Task<AgendamentoResponseDto?> ObterPorIdAsync(Guid id);
        Task<AgendamentoResponseDto> CriarAsync(AgendamentoRequestDto createDto);
        Task ConfirmarAsync(Guid id);
        Task CancelarAsync(Guid id);
    }
}
