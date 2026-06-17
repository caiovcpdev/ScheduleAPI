using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Data;
using ScheduleAPI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Repositories
{
    public class ProfissionalRepository : IProfissionalRepository
    {
        private readonly AppDbContext _context;
        public ProfissionalRepository(AppDbContext context) => _context = context;

        public async Task AdicionarAsync(Profissional profissional)
        {
            await _context.Profissionais.AddAsync(profissional);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Profissional profissional)
        {
            _context.Profissionais.Update(profissional);
            await _context.SaveChangesAsync();
        }

        public async Task<Profissional?> ObterPorIdAsync(Guid id)
            => await _context.Profissionais.FindAsync(id);

        public async Task<List<Servico>> ObterServicoPorProfissional(Guid id)
        {
            var result = await _context.Servicos
            .Where(s => s.Profissionais.Any(p => p.Id == id))
            .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Profissional>> ObterTodosAsync()
            => await _context.Profissionais.ToListAsync();
    }
}
