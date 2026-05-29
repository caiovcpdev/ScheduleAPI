using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.DTOs.Agendamento
{
    //Records são imutáveis por padrão e têm igualdade por valor. Perfeitos para DTOs que só carregam dados sem comportamento.
    public record AgendamentoResponseDto(
        Guid Id,
        string ClienteNome,
        string ProfissionalNome,
        string ServicoNome,
        decimal ServicoPreco,
        DateTime DataHoraInicio,
        DateTime DataHoraFim,
        string Status,
        string? Observacao,
        DateTime CreatedAt
    );
}
