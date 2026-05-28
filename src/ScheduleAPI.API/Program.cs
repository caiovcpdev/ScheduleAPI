using Microsoft.EntityFrameworkCore;
using ScheduleAPI.Infrastructure.Data;
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


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();