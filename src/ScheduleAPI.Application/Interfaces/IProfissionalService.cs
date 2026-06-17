using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.DTOs.Servico;

namespace ScheduleAPI.Application.Interfaces
{
    public interface IProfissionalService
    {
        Task<IEnumerable<ProfissionalResponseDto>> ObterTodosAsync();
        Task<ProfissionalResponseDto?> ObterPorIdAsync(Guid id);
        Task<ProfissionalResponseDto> CriarAsync(ProfissionalRequestDto dto);
        Task<DisponibilidadeResponseDto> ObterDisponibilidadeAsync(Guid id, DateTime data, int intervaloEmMinutos = 30);
        Task<IEnumerable<ServicoResponseDto>> ObterServicoPorProfissionalAsync(Guid servicoId);
    }
}
