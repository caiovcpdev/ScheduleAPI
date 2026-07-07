using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScheduleAuth.API.Filters;
using ScheduleAuth.Application.DTOs.Auth.Internal;
using ScheduleAuth.Application.Interfaces;

namespace ScheduleAuth.API.Controllers
{
    [ApiController]
    [Route("api/internal/usuarios")]
    public class UsuariosInternosController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsuariosInternosController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [InternalApiKey]
        public async Task<IActionResult> CriarParaProfissional([FromBody] CriarUsuarioParaProfissionalRequest request)
        {
            var result = await _authService.CriarUsuarioParaProfissionalAsync(request);
            return Ok(result);
        }
    }
}
