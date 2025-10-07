using ReferralApi.Models;

namespace ReferralApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token);
    }
}
