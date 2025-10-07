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
                // Verifica se já existem usuários no banco
                if (context.users.Any())
                {
                    return; // Banco já foi seedado
                }

                // Criar usuários iniciais
                var users = new[]
                {
                    new User("Ana Silva", "ana.silva@email.com", "Senha1234"),
                    new User("Bruno Oliveira", "bruno.oliveira@email.com", "Senha1234"),
                    new User("Carla Santos", "carla.santos@email.com", "Senha1234"),
                    new User("Diego Souza", "diego.souza@email.com", "Senha1234"),
                    new User("Elena Costa", "elena.costa@email.com", "Senha1234")
                };

                // Adicionar pontos aleatórios para simular indicações
                var random = new Random();
                foreach (var user in users)
                {
                    user.AddPoints(random.Next(0, 10)); // 0 a 10 pontos
                }

                context.users.AddRange(users);
                context.SaveChanges();

                Console.WriteLine("✅ Seed data criado com sucesso!");
                Console.WriteLine($"📊 {users.Length} usuários criados");
            }
        }
    }
}