using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Application.DTOs.Auth.Refresh
{
    public record RefreshResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
