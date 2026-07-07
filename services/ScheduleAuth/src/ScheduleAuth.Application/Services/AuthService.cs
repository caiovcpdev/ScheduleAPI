using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ScheduleAuth.Application.DTOs.Auth.Internal;
using ScheduleAuth.Application.DTOs.Auth.Login;
using ScheduleAuth.Application.DTOs.Auth.Refresh;
using ScheduleAuth.Application.DTOs.Auth.Usuario;
using ScheduleAuth.Application.Interfaces;
using ScheduleAuth.Application.Settings;
using ScheduleAuth.Domain.Entities;
using ScheduleAuth.Domain.Enums;
using ScheduleAuth.Domain.Repositories;
using System.Security.Cryptography;


namespace ScheduleAuth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUsuarioRepository usuarioRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher<Usuario> passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IOptions<JwtSettings> jwtSettings)
        {
            _usuarioRepository = usuarioRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<UsuarioResponse> CriarUsuarioAsync(UsuarioRequest request)
        {
            if (!Enum.TryParse<RoleUsuario>(request.Role, ignoreCase: true, out var role))
                throw new ArgumentException("Role inválida.");

            // PasswordHasher.HashPassword pede uma instância de Usuario só como "contexto" (não usa os dados dela)
            //var usuarioTemporario = new UsuarioPlaceholder();
            var passwordHash = _passwordHasher.HashPassword(null!, request.Senha);

            var usuario = role switch
            {
                RoleUsuario.Admin => Usuario.CriarAdministrador(request.Nome, request.Email, passwordHash),
                RoleUsuario.Profissional when request.ProfissionalId is not null =>
                    Usuario.CriarParaProfissional(request.Nome, request.Email, passwordHash, request.ProfissionalId.Value),
                _ => throw new ArgumentException("Combinação de Role/ProfissionalId inválida.")
            };

            await _usuarioRepository.AdicionarAsync(usuario);

            return new UsuarioResponse(usuario.Id, usuario.Nome, usuario.Email, usuario.Role.ToString(), usuario.ProfissionalId);
        }

        public async Task<CriarUsuarioParaProfissionalRespose> CriarUsuarioParaProfissionalAsync(CriarUsuarioParaProfissionalRequest request)
        {
            var senhaProvisoria = GerarSenhaProvisoria();
            var passwordHash = _passwordHasher.HashPassword(null!, senhaProvisoria);    

            var usuario = Usuario.CriarParaProfissional(
                request.Nome,
                request.Email, 
                passwordHash, 
                request.ProfissionalId);

            await _usuarioRepository.AdicionarAsync(usuario);

            return new CriarUsuarioParaProfissionalRespose(
                usuario.Id,
                senhaProvisoria);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email.Trim());

            if (usuario is null || !usuario.Ativo)
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Senha);

            if (resultado == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            var (accessToken, expiresAt) = _jwtTokenGenerator.GerarAccessToken(usuario);
            var refreshToken = await GerarERegistrarRefreshTokenAsync(usuario.Id);


            return new LoginResponse(
                accessToken,
                refreshToken.Token,
                expiresAt,
                usuario.Nome,
                usuario.Email,
                usuario.Role.ToString());
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenExistente = await _refreshTokenRepository.ObterPorTokenAsync(refreshToken);

            if (tokenExistente is not null && tokenExistente.EstaAtivo)
            {
                tokenExistente.Revogar();
                await _refreshTokenRepository.AtualizarAsync(tokenExistente);
            }
            // se não existir ou já estiver revogado, o logout é idempotente: não lança erro
        }

        public async Task<RefreshResponse> RefreshAsync(RefreshRequest request)
        {
            var tokenExistente = await _refreshTokenRepository.ObterPorTokenAsync(request.RefreshToken);

            if (tokenExistente is null || !tokenExistente.EstaAtivo)
                throw new UnauthorizedAccessException("Refresh token invalido ou expirado");

            var usuario = await _usuarioRepository.ObterPorIdAsync(tokenExistente.UsuarioId );

            if (usuario is null || !usuario.Ativo)
                throw new UnauthorizedAccessException("Usuário não encontrado ou inativo");

            tokenExistente.Revogar();
            await _refreshTokenRepository.AtualizarAsync(tokenExistente);

            var (accessToken, expiresAt) = _jwtTokenGenerator.GerarAccessToken(usuario);
            var novoRefreshToken = await GerarERegistrarRefreshTokenAsync(usuario.Id);

            return new RefreshResponse(accessToken, novoRefreshToken.Token, expiresAt);

        }

        private async Task<RefreshToken> GerarERegistrarRefreshTokenAsync(Guid usuarioId)
        {
            var tokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshToken = RefreshToken.Gerar(usuarioId, tokenString, _jwtSettings.RefreshExpirationDays);

            await _refreshTokenRepository.AdicionarAsync(refreshToken);

            return refreshToken;
        }
        public async Task<UsuarioResponse> MudaSenhaAsync(UsuarioRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email) ?? throw new KeyNotFoundException($"Cliente com Email {request.Email} não encontrado.");
            usuario.AtualizarSenha(request.Senha);
            await _usuarioRepository.AtualizarAsync(usuario);
            return new UsuarioResponse {usuario.Id, usuario.Nome, usuario.Email, usuario.Role, usuario.ProfissionalId} ;

            throw new NotImplementedException();
        }

        private static string GerarSenhaProvisoria()
        {
            var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(9);
            return Convert.ToBase64String(bytes)[..12].Replace('+', 'A').Replace('/', 'B');
        }
    }
}
