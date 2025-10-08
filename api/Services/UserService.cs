using Microsoft.EntityFrameworkCore;
using ReferralApi.Data;
using ReferralApi.DTOs;
using ReferralApi.Models;

namespace ReferralApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request, string? refCode = null)
        {
            if (await GetUserByEmailAsync(request.email) != null)
                throw new ArgumentException("Email já está em uso.");

            var user = new User(request.name, request.email, request.password);

            // ✅ ADICIONE ESTA VERIFICAÇÃO PARA GARANTIR refCode ÚNICO
            while (await GetUserByRefCodeAsync(user.refCode) != null)
            {
                // Se o refCode já existe, gere um novo
                user.RegenerateRefCode(); // Você precisa adicionar este método na classe User
            }

            if (!string.IsNullOrEmpty(refCode))
            {
                var referringUser = await GetUserByRefCodeAsync(refCode);
                if (referringUser != null)
                {
                    user.referredById = referringUser.id;
                    referringUser.AddPoints(1);
                    _context.users.Update(referringUser);
                    Console.WriteLine($"🎯 Usuário {user.email} foi indicado por {referringUser.name}");
                }
                else
                {
                    Console.WriteLine($"⚠️ RefCode não encontrado: {refCode}");
                }
            }

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return MapToUserResponse(user);
        }
        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await GetUserByEmailAsync(request.email);

            if (user == null || !user.VerifyPassword(request.password))
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            return user.id.ToString(); // ✅ CORREÇÃO: Retorna o ID do usuário encontrado
        }

        public async Task<UserResponse> GetUserProfileAsync(int userId) 
        {
            var user = await _context.users
                .Include(u=> u.referredBy)
                .FirstOrDefaultAsync(u => u.id == userId);

            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return MapToUserResponse(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.email == email);
        }
        public async Task<User> GetUserByRefCodeAsync(string refCode)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.refCode == refCode);
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.users
                .FirstOrDefaultAsync(u => u.id == userId);
        }
        private UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                id = user.id,
                name = user.name,
                email = user.email,
                refCode = user.refCode,
                points = user.points,
                refLink = $"http://localhost:3000/register?ref={user.refCode}",
                referredByName = user.referredBy?.name
            };
        }
    }
}
