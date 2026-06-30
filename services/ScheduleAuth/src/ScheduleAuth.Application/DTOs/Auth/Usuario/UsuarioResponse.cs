using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Application.DTOs.Auth.Usuario
{
    public record UsuarioResponse(Guid Id, string Nome, string Email, string Role, Guid? ProfissionalId);
}
