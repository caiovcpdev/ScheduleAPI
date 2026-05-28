using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScheduleAPI.Domain.Entities
{
    public enum StatusAgendamento
    {
        Pendente,
        Confirmado,
        Cancelado,
        Concluido
    }
    public class Agendamento : BaseEntity
    {
        public Guid ClienteId { get; private set; }
        public Guid ProfissionalId { get; private set; }
        public Guid ServicoId { get; private set; }

        public DateTime DataHorarioInicio { get; private set; }
        public DateTime DataHorarioFim { get; private set; }
        public StatusAgendamento Status {  get; private set; }
        public string? Observacao { get; private set; }

        //Propriedadesd e Navegação EF
        public Cliente? Cliente {  get; private set; }
        public Profissional? Profissional {  get; private set; }
        public Servico? Servico {  get; private set; }

        private Agendamento() { }

        public Agendamento (Guid clienteId, Guid profissionalId, Guid servicoId, DateTime dataHorarioInicio, DateTime dataHorarioFim, StatusAgendamento status, string? observacao, Cliente? cliente, Profissional? profissional, Servico? servico)
        {
            Validar(dataHorarioInicio);
            ClienteId = clienteId;
            ProfissionalId = profissionalId;
            ServicoId = servicoId;
            DataHorarioInicio = dataHorarioInicio;
            DataHorarioFim = dataHorarioFim;
            Status = status;
            Observacao = observacao;
            Cliente = cliente;
            Profissional = profissional;
            Servico = servico;
        }
        public void Confirmar()
        {
            if (Status != StatusAgendamento.Pendente)
                throw new InvalidOperationException("Apenas agendamentos pedente podems ser confirmados");
            Status = StatusAgendamento.Confirmado;
            UpdatedAt = DateTime.Now;
        }

        public void Cancelar()
        {
            if (Status == StatusAgendamento.Concluido)
                throw new InvalidOperationException("Não é possivel cancelar um agendamento concluído.");
            Status = StatusAgendamento.Cancelado;
            UpdatedAt = DateTime.UtcNow;
        }

        //Regra de negócio central. Dois agendamentos conflitam se um começa antes do outro terminar.
        public bool ConflitaCom(Agendamento outro)
        {
            if (outro.ProfissionalId != ProfissionalId) return false;
            if (outro.Status == StatusAgendamento.Cancelado) return false;

            return DataHorarioInicio < outro.DataHorarioFim && DataHorarioFim > outro.DataHorarioInicio;
        }
        private static void Validar(DateTime dataHorarioInicio) 
        {
            if (dataHorarioInicio < DateTime.UtcNow) throw new ArgumentException("Não é possível agendar para uma data passada.");
        }

    }
}
