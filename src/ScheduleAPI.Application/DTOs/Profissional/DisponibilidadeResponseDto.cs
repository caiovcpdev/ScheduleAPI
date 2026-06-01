using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.DTOs.Profissional
{
    public record SlotDisponivel(
        TimeSpan HorarioInicio,
        TimeSpan HorarioFim,
        bool Disponivel
    );
    public record DisponibilidadeResponseDto
    (
      Guid ProfissionalId,
      string ProfissionalNome,
      DateTime Data,
      TimeSpan InicioExpediente,
      TimeSpan FimExpediente,
      IEnumerable<SlotDisponivel> Slots
    );
}
