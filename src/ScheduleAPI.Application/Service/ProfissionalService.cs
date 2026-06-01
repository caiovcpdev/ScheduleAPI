using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Service
{
    public class ProfissionalService : IProfissionalService 
    {
        private readonly IProfissionalRepository _repository;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public ProfissionalService(IProfissionalRepository repository, IAgendamentoRepository agendamentoRepository) 
        { 
            _repository = repository; 
            _agendamentoRepository = agendamentoRepository;
        } 

        public async Task<ProfissionalResponseDto> CriarAsync(ProfissionalRequestDto dto)
        {
            var profissional = new Profissional(dto.Nome, dto.Email, dto.Especialidade, dto.InicioExpediente, dto.FimExpediente);

            await _repository.AdicionarAsync(profissional);
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
        private static ProfissionalResponseDto ToDto(Profissional p) => new(p.Id, p.Nome, p.Email, p.Especialidade, p.InicioExpediente, p.FimExpediente);
    }
}
