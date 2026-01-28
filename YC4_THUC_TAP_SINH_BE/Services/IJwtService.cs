using System.Security.Claims;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user, List<string> roles, List<string> functions);
        ClaimsPrincipal ValidateToken(string token);
    }
}
