using Microsoft.EntityFrameworkCore;
using ScheduleAuth.Domain.Entities;
using ScheduleAuth.Domain.Repositories;
using ScheduleAuth.Infrastructure.Data;


namespace ScheduleAuth.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context) => _context = context;

        public async Task AdicionarAsync(RefreshToken refreshToken)
        {
            await _context.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> ObterPorTokenAsync(string token)
            => await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }
}
