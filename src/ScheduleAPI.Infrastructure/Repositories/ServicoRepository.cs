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
    public class ServicoRepository : IServicoRepository
    {
        private readonly AppDbContext _context;
        public ServicoRepository(AppDbContext context) => _context = context;

        public async Task AdicionarAsync(Servico servico)
        {
            await _context.Servicos.AddAsync(servico);
            await _context.SaveChangesAsync();  
        }

        public async Task<Servico?> ObterPorIdAsync(Guid id)
            => await _context.Servicos.FindAsync(id);

        public async Task<List<Profissional>> ObterProfissionalPorServico(Guid servicoId)
        {
            return await _context.Profissionais
           .Where(p => p.Servicos.Any(s => s.Id == servicoId))
           .ToListAsync();
        }

        public async Task<IEnumerable<Servico>> ObterTodosAsync()
            => await _context.Servicos.ToListAsync();
    }
}
