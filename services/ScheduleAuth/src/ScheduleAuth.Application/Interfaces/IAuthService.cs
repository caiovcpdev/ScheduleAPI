using ScheduleAuth.Application.DTOs.Auth.Login;
using ScheduleAuth.Application.DTOs.Auth.Refresh;
using ScheduleAuth.Application.DTOs.Auth.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RefreshResponse> RefreshAsync(RefreshRequest request);
        Task LogoutAsync(string refreshToken);
        Task<UsuarioResponse> CriarUsuarioAsync(UsuarioRequest request);  // admin cria qualquer usuário manualmente
    }
}
