using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleAuth.Application.DTOs.Auth.Login;
using ScheduleAuth.Application.DTOs.Auth.Refresh;
using ScheduleAuth.Application.DTOs.Auth.Usuario;
using ScheduleAuth.Application.Interfaces;

namespace ScheduleAuth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);   
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<RefreshResponse>> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _authService.RefreshAsync(request);
            return Ok(result);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Logout([FromBody] RefreshRequest request)
        {
            await _authService.LogoutAsync(request.RefreshToken);
            return NoContent();
        }

        [HttpPost("muda-senha")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioResponse>> MudaSenha([FromBody] LoginRequest request)
        {
            var result = await _authService.MudaSenhaAsync(request);
            return Ok(result);
        }

        [HttpPost("usuarios")]
        public async Task<ActionResult<UsuarioResponse>> CriarUsuario([FromBody] UsuarioRequest request)
        {
            var result = await _authService.CriarUsuarioAsync(request);
            return CreatedAtAction(nameof(CriarUsuario), new { id = result.Id }, result);
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] UsuarioRequest request)
        {
            var cliente = await _authService.AtualizarAsync(request);
            return Ok(cliente);
        }
    }
}
