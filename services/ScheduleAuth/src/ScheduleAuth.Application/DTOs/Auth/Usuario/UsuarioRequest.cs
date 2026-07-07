using ScheduleAuth.Domain.Enums;

namespace ScheduleAuth.Application.DTOs.Auth.Usuario
{
    public record UsuarioRequest(string Nome, string Email, string Senha, string Role, Guid? ProfissionalId);
}
