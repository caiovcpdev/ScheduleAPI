using ScheduleAPI.Application.DTOs.Profissional;

namespace ScheduleAPI.Application.Interfaces
{
    public interface IProfissionalService
    {
        Task<IEnumerable<ProfissionalResponseDto>> ObterTodosAsync();
        Task<ProfissionalResponseDto?> ObterPorIdAsync(Guid id);
        Task<ProfissionalResponseDto> CriarAsync(ProfissionalRequestDto dto);
        //Task<ProfissionalResponseDto> AtualizarAsync(Guid id, ProfissionalResponseDto dto);
    }
}
