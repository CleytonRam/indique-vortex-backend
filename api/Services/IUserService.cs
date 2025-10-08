using ReferralApi.DTOs;
using ReferralApi.Models;

namespace ReferralApi.Services
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request, string? refCode = null);
        Task<string> LoginAsync(LoginRequest request);
        Task<UserResponse> GetUserProfileAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByRefCodeAsync(string refCode);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
