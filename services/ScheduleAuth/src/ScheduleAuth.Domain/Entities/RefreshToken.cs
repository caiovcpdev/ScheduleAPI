using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; } = null;
        public string Token { get; private set; } = string.Empty;
        public DateTime ExpireAt { get; private set; }
        public DateTime CreateAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public bool EstaExpirado => DateTime.Now >= ExpireAt;
        public bool EstaRevogado => RevokedAt.HasValue;
        public bool EstaAtivo => !EstaExpirado && EstaRevogado;
        
        //EF
        private RefreshToken() { }

        private RefreshToken(Guid usuarioId, string token, DateTime expiresAt)
        {
            UsuarioId = usuarioId;
            Token = token;
            ExpireAt = expiresAt;
            CreateAt = DateTime.UtcNow;
        }

        public static RefreshToken Gerar(Guid usuarioId, string token, int diasDeValidade)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token não pode ser vazio");

            return new RefreshToken(usuarioId, token, DateTime.UtcNow.AddDays(diasDeValidade));
        }

        public void Revogar() 
        {
            if (EstaRevogado) throw new InvalidOperationException("Refresh token já foi revogado");
            
            RevokedAt = DateTime.UtcNow;
        }
    }
}
