using ScheduleAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> ObterPorTokenAsync(string token);
        Task AdicionarAsync(RefreshToken refreshToken);
        Task AtualizarAsync(RefreshToken refreshToken);
    }
}
