using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.DTOs.Cliente
{
    public record ClienteResponseDto(
        Guid  Id,
        string Nome,
        string Email,
        string Telefone,
        DateTime CreatedAt
    );
}
