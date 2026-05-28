using ScheduleAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Interfaces
{
    public interface IProfissionalRepository
    {
        Task<Profissional?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Profissional>> ObterTodosAsync();
        Task AdicionarAsync(Profissional profissional);
        Task AtualizarAsync(Profissional profissional);
    }
}
