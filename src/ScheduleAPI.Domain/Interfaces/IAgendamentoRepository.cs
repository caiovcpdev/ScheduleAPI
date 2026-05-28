using ScheduleAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Interfaces
{
    public interface IAgendamentoRepository
    {
        Task<Agendamento?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Agendamento>> ObterTodosAsync();
        Task<IEnumerable<Agendamento>> ObterPorProfissionalAsync(Guid profissionalId, DateTime data);
        Task<bool> ExisteConflitoAsync(Guid profissionalId, DateTime inicio, DateTime fim, Guid? ignorarId = null);
        Task AdicionaAsync(Agendamento agendamento);
        Task AtualizarAsync(Agendamento agendamento);
    }
}
