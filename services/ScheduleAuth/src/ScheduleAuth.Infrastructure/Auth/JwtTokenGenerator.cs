using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScheduleAuth.Application.Interfaces;
using ScheduleAuth.Application.Settings;
using ScheduleAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAuth.Infrastructure.Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(IOptions<JwtSettings> settigns) => _settings = settigns.Value;
        public (string Token, DateTime ExpiresAt) GerarAccessToken(Usuario usuario)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Email, usuario.Email),
                new(ClaimTypes.Role, usuario.Role.ToString())
            };

            if (usuario.ProfissionalId is not null)
                claims.Add(new Claim("ProfissionalId", usuario.ProfissionalId.Value.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
    }
}
