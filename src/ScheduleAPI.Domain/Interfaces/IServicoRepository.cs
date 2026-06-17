using ScheduleAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Interfaces
{
    public interface IServicoRepository
    {
        Task<Servico?> ObterPorIdAsync(Guid id); 
        Task<IEnumerable<Servico>> ObterTodosAsync();
        Task AdicionarAsync(Servico servico);
        Task<List<Profissional>> ObterProfissionalPorServico(Guid servicoId);

    }
}
