using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Application.DTOs.Auth.Login
{
    public record LoginRequest(string Email, string Senha);
}
