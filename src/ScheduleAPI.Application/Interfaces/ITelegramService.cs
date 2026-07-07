using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Interfaces
{
    public interface ITelegramService
    {
        Task EnviarMensagemAsync (string mensagem);
    }
}
