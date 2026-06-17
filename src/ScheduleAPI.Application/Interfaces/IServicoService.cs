using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.DTOs.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Interfaces
{
    public interface IServicoService
    {
        Task<IEnumerable<ServicoResponseDto>> ObterTodosAsync();
        Task<ServicoResponseDto?> ObterPorIdAsync(Guid id);
        Task<ServicoResponseDto> CriarAsync(ServicoRequestDto dto);

        Task<IEnumerable<ProfissionalResponseDto>> ObterProfissionalPorServicoAsync(Guid profissionalId);
    }
}
