using Microsoft.EntityFrameworkCore;
using ScheduleAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Auth"); // Tudo desde context cai em "Auth.Usuario", "Auth.RefreshTokens"

            modelBuilder.Entity<Usuario>(builder =>
            {
                builder.ToTable("Usuarios");
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Nome).IsRequired().HasMaxLength(150);
                builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
                builder.HasIndex(u => u.Email).IsUnique();
                builder.Property(u => u.PasswordHash).IsRequired();
                builder.Property(u => u.Role).IsRequired();
                builder.HasIndex(u => u.ProfissionalId).IsUnique(); // sem HasOne/WithOne: ProfissionalId é só um inteiro guardado, sem FK de banco
            });

            modelBuilder.Entity<RefreshToken>(builder => {
                builder.ToTable("RefreshTokens");
                builder.HasKey(rt => rt.Id);
                builder.HasKey(rt => rt.Id);
                builder.Property(rt => rt.Token).IsRequired().HasMaxLength(200);
                builder.HasIndex(rt => rt.Token).IsUnique();
                builder.HasOne(rt => rt.Usuario).WithMany().HasForeignKey(rt => rt.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
