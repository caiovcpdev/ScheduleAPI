using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Application.DTOs.Auth.Login
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        string Nome,
        string Email,
        string Role);
}
