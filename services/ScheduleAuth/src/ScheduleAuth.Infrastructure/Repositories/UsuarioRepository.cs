using Microsoft.EntityFrameworkCore;
using ScheduleAuth.Domain.Entities;
using ScheduleAuth.Domain.Repositories;
using ScheduleAuth.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context) => _context = context;

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Usuario?> ObterPorProfissionalIdAsync(Guid profissionalId)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.ProfissionalId == profissionalId);
    }
}
