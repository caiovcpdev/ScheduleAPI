using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Interfaces
{
    public interface IEmailService
    {
        Task EnviarAsync(string destinatario, string nome, string assunto, string corpo);
    }
}
