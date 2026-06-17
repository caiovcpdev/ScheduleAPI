using ScheduleAPI.Application.DTOs.Profissional;
using ScheduleAPI.Application.DTOs.Servico;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Application.Service
{
    public class ServicoService : IServicoService
    {
        private readonly IServicoRepository _repository;
        public ServicoService(IServicoRepository repository) => _repository = repository;

        public async Task<ServicoResponseDto> CriarAsync(ServicoRequestDto dto)
        {
            var servico = new Servico(dto.Nome, dto.Descricao, dto.Preco, dto.DuracaoEmMinutos);
            await _repository.AdicionarAsync(servico);
            return ToDto(servico);
        }

        public async Task<ServicoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var servico = await _repository.ObterPorIdAsync(id);
            return servico is null ? null : ToDto(servico);
        }

        public async Task<IEnumerable<ServicoResponseDto>> ObterTodosAsync()
        {
            var servicos = await _repository.ObterTodosAsync();
            return servicos.Select(ToDto);
        }

        public async Task<IEnumerable<ProfissionalResponseDto>> ObterProfissionalPorServicoAsync(Guid profissionalId)
        {
            var profissionais = await _repository.ObterProfissionalPorServico(profissionalId);
            return profissionais.Select(ToProfissionalDto);
        }
        
        //Mappers
        private static ServicoResponseDto ToDto(Servico s) => new(
            s.Id,
            s.Nome,
            s.Descricao, 
            s.Preco, 
            s.DuracaoEmMinutos);

        private static ProfissionalResponseDto ToProfissionalDto(Profissional p) => new(
            p.Id,
            p.Nome,
            p.Email,
            p.Especialidade,
            p.InicioExpediente,
            p.FimExpediente);

    }
}
