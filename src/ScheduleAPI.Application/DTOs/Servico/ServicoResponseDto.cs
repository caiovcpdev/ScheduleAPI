using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.DTOs.Servico
{
    public record ServicoResponseDto(
        Guid Id,
        string Nome,
        string Descricao,
        decimal Preco,
        int DuracaoEmMinutos
    );
}
