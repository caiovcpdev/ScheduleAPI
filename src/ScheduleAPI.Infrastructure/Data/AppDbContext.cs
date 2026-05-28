using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Profissional> Profissionais => Set<Profissional>();
        public DbSet<Servico> Servicos => Set<Servico>();
        public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(e => 
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Nome).IsRequired().HasMaxLength(100);
                e.Property(c => c.Email).IsRequired().HasMaxLength(150);
                e.HasIndex(c => c.Email).IsUnique();
                e.Property(c => c.Telefone).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<Profissional>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
                e.Property(p => p.Email).IsRequired().HasMaxLength(150);
                e.HasIndex(p => p.Email).IsUnique();
                e.Property(p => p.Especialidade).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Servico>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.Nome).IsRequired().HasMaxLength(100);
                e.Property(s => s.Preco).HasPrecision(10, 2);
            });

            modelBuilder.Entity<Agendamento>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Status).HasConversion<string>();

                e.HasOne(a => a.Cliente)
                .WithMany()
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.Profissional)
                .WithMany()
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.Servico)
                .WithMany()
                .HasForeignKey(a => a.ServicoId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
