using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Application.Templates;
using ScheduleAPI.Infrastructure.Interfaces;


namespace ScheduleAPI.Infrastructure.BackgroundServices
{
    public class LembreteBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _scopeFactory;
        private readonly ILogger<LembreteBackgroundService> _logger;

        private readonly TimeSpan _intervalo = TimeSpan.FromHours(1);
        //private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(1);

        public LembreteBackgroundService(IServiceProvider scopeFactory, ILogger<LembreteBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LembreteBackgroundService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await EnviarLembretesAsync();
                await Task.Delay(_intervalo, stoppingToken);
            }
        }

        private async Task EnviarLembretesAsync()
        {
            _logger.LogInformation("Verificando agendamentos de amanhã... {hora}", DateTime.Now);

            using var scope = _scopeFactory.CreateScope();

            var agendamentoRepo = scope.ServiceProvider.GetRequiredService<IAgendamentoRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            try
            {
                var agendamentos = await agendamentoRepo.ObterAgendamentosDeAmanha();
                var lista = agendamentos.ToList();

                _logger.LogInformation("{total} agendamento(s) encontrado(s) para amanhã.", lista.Count);

                foreach (var agendamento in lista)
                {
                    try
                    {
                        var corpo = EmailLembreteTemplate.Gerar(
                            agendamento.Cliente!.Nome, 
                            agendamento.Profissional!.Nome,
                            agendamento.Servico!.Nome,
                            agendamento.DataHoraInicio,
                            agendamento.Servico!.Preco
                        );

                        await emailService.EnviarAsync(
                            agendamento.Cliente!.Email, 
                            agendamento.Cliente!.Nome, 
                            "Lembrete: você tem um agendamento amanhã!", 
                            corpo
                        );

                        _logger.LogInformation("E-mail enviado para {email}", agendamento.Cliente!.Email);
                    }
                    catch (Exception ex) { _logger.LogError(ex, "Erro ao enviar e-mail para {email}", agendamento.Cliente!.Email); }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral ao processar lembretes.");
            }
        }
    }
}
