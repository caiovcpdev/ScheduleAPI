using ScheduleAPI.Application.DTOs.Agendamento;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Application.Templates;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Interfaces;

namespace ScheduleAPI.Application.Service
{
    public class AgendamentoService : IAgendamentoSerivce
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly ITelegramService _telegramService;
        private readonly IEmailService _emailService;
        public AgendamentoService(
            IAgendamentoRepository agendamentoRepository,
            IProfissionalRepository profissionalRepository,
            IServicoRepository servicoRepository,
            IClienteRepository clienteRepository,
            ITelegramService telegramService,
            IEmailService emailService)
        {
            _agendamentoRepository = agendamentoRepository;
            _profissionalRepository = profissionalRepository;
            _servicoRepository = servicoRepository;
            _clienteRepository = clienteRepository;
            _telegramService = telegramService;
            _emailService = emailService;
        }

        public async Task CancelarAsync(Guid id)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"Agendamento com id {id} não encontrado.");

            agendamento.Cancelar();
            await _agendamentoRepository.AtualizarAsync(agendamento);
        }

        public async Task ConfirmarAsync(Guid id)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"Agendamento com id {id} não encontrado.");

            agendamento.Confirmar();
            await _agendamentoRepository.AtualizarAsync(agendamento);
            
            await _telegramService.EnviarMensagemAsync( $"✨ Olá {agendamento.Profissional.Nome}! ✨\n\n" +
                                                        $"✅ Seu agendamento foi confirmado!\n\n" +
                                                        $"👩‍⚕️ Cliente: {agendamento.Cliente.Nome}\n" +
                                                        $"🕒 Horário: {agendamento.DataHoraInicio:dd/MM/yyyy HH:mm}\n\n" +
                                                        $"📍 Fique atento. Até breve!");

            var corpo = EmailLembreteTemplate.Gerar(
                       agendamento.Cliente.Nome,
                       agendamento.Profissional.Nome,
                       agendamento.Servico.Nome,
                       agendamento.DataHoraInicio,
                       agendamento.Servico.Preco);

            await _emailService.EnviarAsync(
                agendamento.Cliente.Email,
                agendamento.Cliente.Nome,
                "Lembrete para o agendamento",
                corpo);
        }
        public async Task<AgendamentoResponseDto> CriarAsync(AgendamentoRequestDto dto)
        {
            //1. Valida se Cliente, Profissional e Serviço existem
            var cliente = await _clienteRepository.ObterPorIdAsync(dto.ClienteId) ?? throw new KeyNotFoundException($"Cliente com id {dto.ClienteId} não encontrado.");
            var profissional = await _profissionalRepository.ObterPorIdAsync(dto.ProfissionalId) ?? throw new KeyNotFoundException($"Profissional com id {dto.ProfissionalId} não encontrado.");
            var servico = await _servicoRepository.ObterPorIdAsync(dto.ServicoId) ?? throw new KeyNotFoundException($"Servico com id {dto.ServicoId} não encontrado.");

            //2. Valida se o profissional atende no horário solicitado
            var horario = dto.DataHoraInicio.TimeOfDay;
            if(!profissional.EstaDisponivelNaHora(horario))
                throw new InvalidOperationException($"Profissional não atende nesse horário. Expediente: {profissional.InicioExpediente} às {profissional.FimExpediente}.");

            //3. Valida se o profissional já possui um agendamento nesse horário
            var dataFim = dto.DataHoraInicio.AddMinutes(servico.DuracaoEmMinutos);
            var temConflito = await _agendamentoRepository.ExisteConflitoAsync(dto.ProfissionalId, dto.DataHoraInicio, dataFim);

            if (temConflito)
                throw new InvalidOperationException("Profissional já possui um agendamento nesse horário.");

            //4. Cria o agendamento
            var agendamento = new Agendamento(
                dto.ClienteId,
                dto.ProfissionalId,
                dto.ServicoId,
                dto.DataHoraInicio,
                servico.DuracaoEmMinutos,
                dto.Observacao
            );

            await _agendamentoRepository.AdicionaAsync(agendamento);
            return ToDto(agendamento);
        }

        public async Task<AgendamentoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(id);
            return agendamento is null ? null : ToDto(agendamento);
        }

        public async Task<IEnumerable<AgendamentoResponseDto>> ObterTodosAsync()
        {
            var agendamentos = await _agendamentoRepository.ObterTodosAsync();
            return agendamentos.Select(ToDto);
        }

        private static AgendamentoResponseDto ToDto(Agendamento a) => new(
            a.Id,
            a.Cliente!.Nome,
            a.Profissional!.Nome,
            a.Servico!.Nome,
            a.Servico!.Preco,
            a.DataHoraInicio,
            a.DataHoraFim,
            a.Status.ToString(),
            a.Observacao,
            a.CreatedAt
        );
    }
}
