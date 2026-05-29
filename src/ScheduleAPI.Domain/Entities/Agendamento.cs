namespace ScheduleAPI.Domain.Entities;

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

    public DateTime DataHoraInicio { get; private set; }
    public DateTime DataHoraFim { get; private set; }
    public StatusAgendamento Status { get; private set; }
    public string? Observacao { get; private set; }

    // Navigation properties (EF Core)
    public Cliente? Cliente { get; private set; }
    public Profissional? Profissional { get; private set; }
    public Servico? Servico { get; private set; }

    private Agendamento() { }

    public Agendamento(Guid clienteId, Guid profissionalId, Guid servicoId,
                       DateTime dataHoraInicio, int duracaoEmMinutos, string? observacao = null)
    {
        if (dataHoraInicio < DateTime.UtcNow)
            throw new ArgumentException("Não é possível agendar para uma data passada.");

        ClienteId = clienteId;
        ProfissionalId = profissionalId;
        ServicoId = servicoId;
        DataHoraInicio = dataHoraInicio;
        DataHoraFim = dataHoraInicio.AddMinutes(duracaoEmMinutos);
        Status = StatusAgendamento.Pendente;
        Observacao = observacao;
    }

    public void Confirmar()
    {
        if (Status != StatusAgendamento.Pendente)
            throw new InvalidOperationException("Apenas agendamentos pendentes podem ser confirmados.");
        Status = StatusAgendamento.Confirmado;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        if (Status == StatusAgendamento.Concluido)
            throw new InvalidOperationException("Não é possível cancelar um agendamento já concluído.");
        Status = StatusAgendamento.Cancelado;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ConflitaCom(Agendamento outro)
    {
        if (outro.ProfissionalId != ProfissionalId) return false;
        if (outro.Status == StatusAgendamento.Cancelado) return false;

        return DataHoraInicio < outro.DataHoraFim && DataHoraFim > outro.DataHoraInicio;
    }
}