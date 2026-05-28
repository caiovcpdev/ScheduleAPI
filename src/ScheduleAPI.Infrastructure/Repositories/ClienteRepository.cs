using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Domain.Entities;
using ScheduleAPI.Infrastructure.Data;
using ScheduleAPI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;
        public ClienteRepository(AppDbContext context) => _context = context;
        

        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
           _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
            => await _context.Clientes.FindAsync(id);

        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
            => await _context.Clientes.ToListAsync();
    }
}
