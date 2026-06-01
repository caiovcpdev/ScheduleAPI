using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Application.Interfaces;
using ScheduleAPI.Application.Service;
using ScheduleAPI.Infrastructure.BackgroundServices;
using ScheduleAPI.Infrastructure.Data;
using ScheduleAPI.Infrastructure.Email;
using ScheduleAPI.Infrastructure.Interfaces;
using ScheduleAPI.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
//builder.Services.AddHostedService<LembreteBackgroundService>();  //Ativar depois de configurar o SMTP


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
app.UseAuthorization();
app.MapControllers();

app.Run();