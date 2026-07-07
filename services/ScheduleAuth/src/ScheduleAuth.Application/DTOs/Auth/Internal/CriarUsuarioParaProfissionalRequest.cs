using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Application.DTOs.Auth.Internal
{
    public record CriarUsuarioParaProfissionalRequest(
        Guid ProfissionalId, 
        string Nome,
        string Email);
}
