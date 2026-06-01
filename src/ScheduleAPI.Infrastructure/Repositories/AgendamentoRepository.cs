using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Data;
using ScheduleAPI.Infrastructure.Interfaces;

namespace ScheduleAPI.Infrastructure.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly AppDbContext _context;

        public AgendamentoRepository(AppDbContext context)
        {
            _context = context  ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AdicionaAsync(Agendamento agendamento)
        {
            await _context.Agendamentos.AddAsync(agendamento); 
            await _context.SaveChangesAsync();  
        }

        public async Task AtualizarAsync(Agendamento agendamento)
        {
            _context.Agendamentos.Update(agendamento);
            await _context.SaveChangesAsync();
        }



        public async Task<Agendamento?> ObterPorIdAsync(Guid id)
            => await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<Agendamento>> ObterTodosAsync()
            => await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .ToListAsync();

        public async Task<IEnumerable<Agendamento>> ObterPorProfissionalAsync(Guid profissionalId, DateTime data)
            => await _context.Agendamentos
                .Where( a => a.ProfissionalId == profissionalId
                        && a.DataHoraInicio == data.Date
                        && a.Status != StatusAgendamento.Cancelado)
                .ToListAsync();

        public async Task<bool> ExisteConflitoAsync(Guid profissionalId, DateTime inicio, DateTime fim, Guid? ignorarId = null)
            => await _context.Agendamentos
                .AnyAsync(a =>
                    a.ProfissionalId == profissionalId &&
                    a.Status != StatusAgendamento.Cancelado &&
                    (ignorarId == null || a.Id != ignorarId) &&
                    a.DataHoraInicio < fim &&
                    a.DataHoraFim > inicio);

        public async Task<IEnumerable<Agendamento>> ObterAgendamentosDeAmanha() 
        {
            var amanha = DateTime.UtcNow.AddDays(1);

            return await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .Where(a => a.DataHoraInicio.Date == amanha.Date && a.Status != StatusAgendamento.Cancelado) //posteriormente buscar apenas os agendamento com status confirmado
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> ObterAgendamentosConfirmadosDoDia(Guid profissionalId, DateTime data)
            => await _context.Agendamentos
                .Include(a => a.Servico)
                .Where( a =>
                        a.ProfissionalId == profissionalId &&
                        a.DataHoraInicio.Date == data.Date &&
                        a.Status != StatusAgendamento.Cancelado)
                .ToListAsync();
    }
}
