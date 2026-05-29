using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.DTOs.Profissional
{
    public record ProfissionalResponseDto(
        Guid Id,
        string Nome,
        string Email,   
        string Especialidade,   
        TimeSpan InicioExpediente,
        TimeSpan FimExpediente
    );
}
