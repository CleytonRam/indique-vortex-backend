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
                throw new ArgumentException("Email já esta em uso.");

            var user = new User(request.name, request.email, request.password);

            if (!string.IsNullOrEmpty(refCode))
            {
                var referringUser = await GetUserByRefCodeAsync(refCode);
                if (referringUser != null)
                {
                    user.referredById = referringUser.id;
                    referringUser.AddPoints(1);
                    _context.users.Update(referringUser);
                }
            }

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return MapToUserResponse(user);
        }
        public async Task<string> LoginAsync(LoginRequest request) 
        {
            var user = await GetUserByEmailAsync(request.email);
            if (user == null || !user.VerifyPassword(request.password)) throw new UnauthorizedAccessException("Credenciais inválidas.");
            return user.id.ToString();
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
