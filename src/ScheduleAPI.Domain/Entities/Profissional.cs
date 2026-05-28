using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Domain.Entities
{
    public class Profissional : BaseEntity
    {
        public string Nome {  get; private set; } = string.Empty;
        public string Email {  get; private set; } = string.Empty;
        public string Especialidade {  get; private set; } = string.Empty;

        //Horario de trabalho do profissional
        public TimeSpan InicioExpediente { get; private set; }
        public TimeSpan FimExpediente { get; private set; }

        //EF
        private Profissional() { }

        public Profissional (string nome, string email, string especialidade, TimeSpan inicioExpediente, TimeSpan fimExpediente)
        {
            Validar(nome, email, especialidade, inicioExpediente, fimExpediente);
            Nome = nome;
            Email = email;
            Especialidade = especialidade;
            InicioExpediente = inicioExpediente;
            FimExpediente = fimExpediente;
        }
        public bool EstaDisponivelNaHora(TimeSpan horario)
        {
            return horario >= InicioExpediente && horario <= FimExpediente;
        }
        private static void Validar(string nome, string email, string especialidade, TimeSpan inicio, TimeSpan fim) 
        {
            if (string.IsNullOrEmpty(nome)) throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email é obrigatório.");
            if (string.IsNullOrEmpty(especialidade)) throw new ArgumentException("Especialidade é obrigatório.");
            if (fim <= inicio) throw new ArgumentException("Fim do expediente deve ser após o inicio");
        }
    }
}
