using ScheduleAuth.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public RoleUsuario Role { get; private set; }
        public bool Ativo { get; private set; }

        public Guid? ProfissionalId { get; private set; }

        //EF Core
        private Usuario() { }

        private Usuario (string nome, string email, string passwordHash, RoleUsuario role, Guid? profissionalId) 
        {
            Validar(nome, email, role, profissionalId);
            ValidarPasswordHash(passwordHash);
            Nome = nome.Trim();
            Email = email.Trim();
            PasswordHash = passwordHash;
            Role = role;
            ProfissionalId = profissionalId;
            Ativo = true;
        } 

        public static Usuario CriarAdministrador(string nome, string email, string passwordHash)
            => new(nome, email, passwordHash, RoleUsuario.Admin, profissionalId: null);

        public static Usuario CriarParaProfissional(string nome, string email, string passwordHash, Guid profissionalId)
            => new(nome, email, passwordHash, RoleUsuario.Profissional, profissionalId);

        public void Atualizar(string nome, string email, RoleUsuario role, Guid? profissionalId, string senha)
        {
            Validar(nome, email, role, profissionalId);
            Nome = nome;
            Email = email;
            Role = role;
            ProfissionalId = profissionalId;
            AtualizarSenha(senha);
        }

        public void AtualizarSenha(string novaPasswordHash)
        {
            ValidarPasswordHash(novaPasswordHash);
            PasswordHash = novaPasswordHash;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        private static void Validar (string nome, string email, RoleUsuario role, Guid? profissionalId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do usuário é obrigatório.");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("Email inválido.");
            
            //Validar vinculo com Role
            if (role == RoleUsuario.Profissional && profissionalId is null)
                throw new ArgumentException("Usuário do tipo Profissional precisa estar vinculado a um ProfissionalId.");

            if (role == RoleUsuario.Admin && profissionalId is not null)
                throw new ArgumentException("Usuário Administrador não deve estar vinculado a um Profissional.");
        }

        private static void ValidarPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash é obrigatório.");   
        }
    }
}
