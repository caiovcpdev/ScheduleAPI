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

        public ProfissionalService(IProfissionalRepository repository) => _repository = repository;

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

        private static ProfissionalResponseDto ToDto(Profissional p) => new(p.Id, p.Nome, p.Email, p.Especialidade, p.InicioExpediente, p.FimExpediente);
    }
}
