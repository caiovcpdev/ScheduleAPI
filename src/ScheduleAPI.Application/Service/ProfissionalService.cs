using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.DTOs.Servico;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Application.Interfaces.Auth;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Interfaces;

namespace ScheduleAPI.Application.Service
{
    public class ProfissionalService : IProfissionalService 
    {
        private readonly IProfissionalRepository _repository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IScheduleAuthClient _scheduleAuthClient;
        private readonly IEmailService _emailService;
        public ProfissionalService(IProfissionalRepository repository, IAgendamentoRepository agendamentoRepository, IServicoRepository servicoRepository, IScheduleAuthClient scheduleAuthClient, IEmailService emailService) 
        { 
            _repository = repository; 
            _agendamentoRepository = agendamentoRepository;
            _servicoRepository = servicoRepository;
            _scheduleAuthClient = scheduleAuthClient;
            _emailService = emailService;
        } 

        public async Task<ProfissionalResponseDto> CriarAsync(ProfissionalRequestDto dto)
        {
            var profissional = new Profissional(dto.Nome, dto.Email, dto.Especialidade, dto.InicioExpediente, dto.FimExpediente);

            await _repository.AdicionarAsync(profissional);

            //Avisar ao AUTH que um profissional foi criado para criar o usuário correspondente e vinculado.
            try
            {
                var senhaProvisoria = await _scheduleAuthClient
                    .CriarUsuarioParaProfissionalAsync(profissional.Id, profissional.Nome, profissional.Email);

                await _emailService.EnviarAsync(
                    destinatario: profissional.Email,
                    nome: profissional.Nome,
                    assunto: "Bem-vindo ao ScheduleAPI — suas credenciais de acesso",
                    corpo: $"""
                        Olá, {profissional.Nome}!

                        Sua conta foi criada no ScheduleAPI.

                        E-mail: {profissional.Email}
                        Senha provisória: {senhaProvisoria}

                        Recomendamos que você troque sua senha após o primeiro login.
                        """);
            }
            catch (Exception ex)
            {
                // O profissional foi criado mas o usuário não — isso precisa ser investigado
                // Por ora: loga o erro e lança uma exceção clara pra quem chamou
                throw new InvalidOperationException(
                    $"Profissional criado, mas houve falha ao criar usuário no ScheduleAuth: {ex.Message}", ex);
            }

            return ToDto(profissional);
        }

        public async Task<ProfissionalResponseDto?> ObterPorIdAsync(Guid id)
        {
            var profissional = await _repository.ObterPorIdAsync(id);
            return profissional is null ? null : ToDto(profissional);
        }
        public async Task<IEnumerable<ProfissionalResponseDto>> ObterTodosAsync()
        {
            var profissionais = await _repository.ObterTodosAsync();
            return profissionais.Select(ToDto);
        }
        public async Task<DisponibilidadeResponseDto> ObterDisponibilidadeAsync(Guid profissionalId, DateTime data, int intervaloEmMinutos = 30)
        {
            //Valida se o profissional existe
            var profissional = await _repository.ObterPorIdAsync(profissionalId) ?? throw new KeyNotFoundException("Profissional não encontrado.");

            //Não é possível consultar disponibilidade para datas passadas
            if (data.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Não é possível consultar disponibilidade para datas passadas.");

            //Busca agendamentos confirmados do dia para o profissional
            var agendamentos = await _agendamentoRepository.ObterAgendamentosConfirmadosDoDia(profissionalId, data);

            var slots = new List<SlotDisponivel>();
            var slotAtual = profissional.InicioExpediente;

            while (slotAtual.Add(TimeSpan.FromMinutes(intervaloEmMinutos)) <= profissional.FimExpediente)
            {
                var slotFim = slotAtual.Add(TimeSpan.FromMinutes(intervaloEmMinutos));

                //Verifica se o slot atual está ocupado por algum agendamento
                var ocupado = agendamentos.Any(a =>
                    a.DataHoraInicio.TimeOfDay < slotFim && 
                    a.DataHoraFim.TimeOfDay > slotAtual
                );

                slots.Add(new SlotDisponivel(slotAtual, slotFim, !ocupado));

                slotAtual = slotFim;
            }

            return new DisponibilidadeResponseDto(
                profissional.Id,
                profissional.Nome,
                data,
                profissional.InicioExpediente,
                profissional.FimExpediente,
                slots
            );

        }
        public async Task<IEnumerable<ServicoResponseDto>> ObterServicoPorProfissionalAsync(Guid servicoId)
        {
            var servicos = await _repository.ObterServicoPorProfissional(servicoId);
            return servicos.Select(ToServicoDto);
        }
        public async Task<ProfissionalResponseDto> AtualizarAsync(Guid id, ProfissionalRequestDto dto)
        {
            var profissional = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"Profissional com id {id} não encontrado.");

            profissional.Atualizar(dto.Nome, dto.Email, dto.Especialidade, dto.InicioExpediente, dto.FimExpediente);
            await _repository.AtualizarAsync(profissional);
            return ToDto(profissional);
        }
        public async Task VincularServicoAsync(Guid profissionalId, Guid servicoId)
        {
            var profissional = await _repository.ObterComServicoAsync(profissionalId) ?? throw new KeyNotFoundException("Profissional não encontrado.");
            var servico = await _servicoRepository.ObterPorIdAsync(servicoId) ?? throw new KeyNotFoundException("Serviço não encontrado.");

            profissional.AdicionarServico(servico);

            await _repository.AtualizarAsync(profissional);

        }
        private static ServicoResponseDto ToServicoDto(Servico s) => new(s.Id, s.Nome, s.Descricao, s.Preco, s.DuracaoEmMinutos);
        private static ProfissionalResponseDto ToDto(Profissional p) => new(p.Id, p.Nome, p.Email, p.Especialidade, p.InicioExpediente, p.FimExpediente);
    }
}
