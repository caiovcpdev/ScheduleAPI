using ScheduleAPI.Application.DTOs.Cliente;
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
        Task<ProfissionalResponseDto> AtualizarAsync(Guid id, ProfissionalRequestDto dto);
        Task<IEnumerable<ServicoResponseDto>> ObterServicoPorProfissionalAsync(Guid servicoId);
        Task VincularServicoAsync(Guid profissionalId, Guid servicoId);   
    }
}
