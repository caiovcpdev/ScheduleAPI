using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.DTOs.Agendamento
{
    public record AgendamentoRequestDto(
        Guid ClienteId,
        Guid ProfissionalId,
        Guid ServicoId,
        DateTime DataHoraInicio,
        string? Observacao
    );
}
