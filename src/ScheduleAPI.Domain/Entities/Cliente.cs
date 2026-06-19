using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty; 
        public string Telefone { get; private set; } = string.Empty;

        // O EF Precisa de construtor vazio
        private Cliente() { }

        public Cliente(string nome, string email, string telefone)
        {
            Validar(nome, email, telefone);
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }

        public void Atualizar(string nome, string email, string telefone)
        {
            Validar(nome, email, telefone);
            Nome = nome;
            Email = email; 
            Telefone = telefone;
            UpdatedAt = DateTime.Now;
        }

        private static void Validar(string nome, string email, string telefone)
        {
            if (string.IsNullOrEmpty(nome)) throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email é obrigatório.");
            if (string.IsNullOrEmpty(telefone)) throw new ArgumentException("Telefone é obrigatório.");
        }
    }
}
