using ScheduleAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorIdAsync(Guid id);
        Task<Usuario?> ObterPorProfissionalIdAsync(Guid profissionalId);
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
    }
}
