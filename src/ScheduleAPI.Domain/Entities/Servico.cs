using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Domain.Entities
{
    public class Servico : BaseEntity
    {
        public string Nome { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public decimal Preco { get; private set; }
        public int DuracaoEmMinutos { get; private set; }
        public ICollection<Profissional> Profissionais { get; private set; } = new List<Profissional>();

        //EF
        Servico() { }

        public Servico(string nome, string descricao, decimal preco, int duracaoEmMinutos)
        {
            Validar(nome, preco, duracaoEmMinutos);
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            DuracaoEmMinutos = duracaoEmMinutos;
        }

        private static void Validar(string nome, decimal preco, int duracaoEmMinutos)
        {
            if (string.IsNullOrEmpty(nome)) throw new ArgumentException("O Nome é obrigátório.");
            if (preco < 0) throw new ArgumentException("Preço não pode ser negativo.");
            if (duracaoEmMinutos <= 0) throw new ArgumentException("Duração deve ser maior que zero.");
        }
    }
}
