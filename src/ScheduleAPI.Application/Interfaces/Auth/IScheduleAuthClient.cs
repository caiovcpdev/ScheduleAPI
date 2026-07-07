using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Interfaces.Auth
{
    public interface IScheduleAuthClient
    {
        Task<string> CriarUsuarioParaProfissionalAsync(Guid profissionalId, string nome, string email);
    }
}
