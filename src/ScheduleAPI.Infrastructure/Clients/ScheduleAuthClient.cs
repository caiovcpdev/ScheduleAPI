using Microsoft.Extensions.Configuration;
using ScheduleAPI.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Infrastructure.Clients
{
    public class ScheduleAuthClient : IScheduleAuthClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        public ScheduleAuthClient(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _apiKey = configuration["ScheduleAuth:ApiKey"];
        }
        public async Task<string> CriarUsuarioParaProfissionalAsync(Guid profissionalId, string nome, string email)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/internal/usuarios")
            {
                Content = JsonContent.Create(new { ProfissionalId = profissionalId, Nome = nome, Email = email })
            };

            request.Headers.Add("X-Internal-Api-Key", _apiKey);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Erro ao criar usuário no ScheduleAuth: {erro}");
            }

            var result = await response.Content.ReadFromJsonAsync<ScheduleAuthResponse>();
            return result!.SenhaProvisoria;
        }

        private record ScheduleAuthResponse(Guid UsuarioId, string SenhaProvisoria);
    }
}
