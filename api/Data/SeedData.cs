using Microsoft.EntityFrameworkCore;
using ReferralApi.Data;
using ReferralApi.Models;

namespace ReferralApi.Data.Seed
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.users.Any())
                {
                    return; 
                }

                var users = new[]
                {
                    new User("Ana Silva", "ana.silva@email.com", "Senha1234"),
                    new User("Bruno Oliveira", "bruno.oliveira@email.com", "Senha1234"),
                    new User("Carla Santos", "carla.santos@email.com", "Senha1234"),
                    new User("Diego Souza", "diego.souza@email.com", "Senha1234"),
                    new User("Elena Costa", "elena.costa@email.com", "Senha1234")
                };

                var random = new Random();
                foreach (var user in users)
                {
                    user.AddPoints(random.Next(0, 10)); 
                }

                context.users.AddRange(users);
                context.SaveChanges();

                Console.WriteLine("✅ Seed data criado com sucesso!");
                Console.WriteLine($"📊 {users.Length} usuários criados");
            }
        }
    }
}