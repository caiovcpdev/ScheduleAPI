using ScheduleAuth.Application.DTOs.Auth.Internal;
using ScheduleAuth.Application.DTOs.Auth.Login;
using ScheduleAuth.Application.DTOs.Auth.Refresh;
using ScheduleAuth.Application.DTOs.Auth.Usuario;


namespace ScheduleAuth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RefreshResponse> RefreshAsync(RefreshRequest request);
        Task LogoutAsync(string refreshToken);
        Task<CriarUsuarioParaProfissionalRespose> CriarUsuarioParaProfissionalAsync(CriarUsuarioParaProfissionalRequest request);
        Task<UsuarioResponse> CriarUsuarioAsync(UsuarioRequest request);// admin cria qualquer usuário manualmente
        Task<UsuarioResponse> MudaSenhaAsync(UsuarioRequest request);  
    }
}
