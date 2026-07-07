using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ScheduleAPI.Application.Interfaces;
using System.Net.Http.Json;


namespace ScheduleAPI.Infrastructure.Notifications.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly HttpClient _http;
        private readonly TelegramOptions _options;
        private readonly IConfiguration _configuration;
        public TelegramService(HttpClient http, IOptions<TelegramOptions> options, IConfiguration configuration)
        {
            _http = http;
            _options = options.Value;
            _configuration = configuration;
        }
        public async Task EnviarMensagemAsync(string mensagem)
        {
            //var chatId = _configuration["Telegram:ChatId"];
            var chatIds = new[]
            {
                "1944582148",
                "2062930291",
                "5652993638"
            };

            foreach (var chatId in chatIds)
            {
                var request = new 
                {
                    chat_id = chatId,
                    text = mensagem
                };

                var response = await _http.PostAsJsonAsync($"/bot{_options.BotToken}/sendMessage", request);
                response.EnsureSuccessStatusCode();
            }
            
        }       
    }
}
