using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScheduleAuth.Application.Interfaces;
using ScheduleAuth.Application.Services;
using ScheduleAuth.Application.Settings;
using ScheduleAuth.Domain.Entities;
using ScheduleAuth.Domain.Repositories;
using ScheduleAuth.Infrastructure.Auth;
using ScheduleAuth.Infrastructure.Data;
using ScheduleAuth.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Configurar tipada
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsHistoryTable("__EFMigrationsHistory_Auth", "Auth")
    );
});

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

//Autenticação - Autorização
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; ;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero // sem tolerância extra de expiração
    };
});

builder.Services.AddAuthorization();

//API Padrão
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


var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        context.Response.StatusCode = error?.Error switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ArgumentException or InvalidProgramException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var mensagem = context.Response.StatusCode == StatusCodes.Status500InternalServerError ? "Ocorreu um erro interno." : error?.Error?.Message;

        await context.Response.WriteAsJsonAsync(new { erro = mensagem });
    });
});



app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scopo = app.Services.CreateScope())
{
    var context = scopo.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scopo.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();
    await AdminSeeder.SeedAdminAsync(context, passwordHasher);
}

app.Run();
