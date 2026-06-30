using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScheduleAuth.Domain.Entities;

namespace ScheduleAuth.Infrastructure.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAdminAsync(AppDbContext context,IPasswordHasher<Usuario> passwordHasher)
        {
            var jaExisteUsuario = await context.Usuarios.AnyAsync();

            if (jaExisteUsuario)
                return;

            var senhaHash = passwordHasher.HashPassword(null, "Nataia@123");
            var admin = Usuario.CriarAdministrador("Administrador", "admin@scheduleapi.com", senhaHash);

            context.Usuarios.Add(admin);   
            await context.SaveChangesAsync();
        } 
    }
}
