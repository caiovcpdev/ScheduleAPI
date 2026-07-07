using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Application.Interfaces.Auth;
using ScheduleAPI.Application.Service;
using ScheduleAPI.Application.Settings;
using ScheduleAPI.Infrastructure.BackgroundServices;
using ScheduleAPI.Infrastructure.Clients;
using ScheduleAPI.Infrastructure.Data;
using ScheduleAPI.Infrastructure.Interfaces;
using ScheduleAPI.Infrastructure.Notifications.Email;
using ScheduleAPI.Infrastructure.Notifications.Telegram;
using ScheduleAPI.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

//Banco
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositorios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

//Services
builder.Services.AddScoped<IClienteSerivce, ClienteService>();
builder.Services.AddScoped<IProfissionalService, ProfissionalService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IAgendamentoSerivce, AgendamentoService>();

//E-mail
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, SmtpEmailService>();

// Background Service para envio de lembretes 
builder.Services.AddHostedService<LembreteBackgroundService>();  

//Telegram
builder.Services.Configure<TelegramOptions>(
    builder.Configuration.GetSection("Telegram"));

builder.Services.AddHttpClient<ITelegramService, TelegramService>(client => {
    client.BaseAddress = new Uri("https://api.telegram.org/");
});

//ScheduleAuth
builder.Services.AddHttpClient<IScheduleAuthClient, ScheduleAuthClient>(client =>
    client.BaseAddress = new Uri(builder.Configuration["ScheduleAuth:BaseUrl"]!));

//JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero // Remove o tempo de tolerância para expiração do token
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("ERRO:");
            Console.WriteLine(context.Exception.ToString());
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Middleware global de tratamento de erros.
// Ele captura qualquer exceção que aconteça durante a requisição,
// define o código de status HTTP apropriado (404, 400 ou 500)
// e retorna uma resposta em formato JSON com a mensagem do erro.
// Isso garante que a API sempre responda de forma padronizada e clara
// em caso de falhas, facilitando o tratamento no frontend.
app.UseExceptionHandler(errorApp => 
{
    errorApp.Run(async context => 
    { 
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (error?.Error is KeyNotFoundException)
            context.Response.StatusCode = 404;
        else if (error?.Error is InvalidProgramException || error?.Error is ArgumentException)
            context.Response.StatusCode = 400;
        else
            context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            error = error?.Error.Message ?? "Ocorreu um erro interno."
        });
    }); 
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication(); //Lê o token JWT do cabeçalho Authorization e valida sua autenticidade e validade.
app.UseAuthorization(); // Verifica se o usuário autenticado tem permissão para acessar o recurso solicitado (endpoint).
app.MapControllers();

app.Run();