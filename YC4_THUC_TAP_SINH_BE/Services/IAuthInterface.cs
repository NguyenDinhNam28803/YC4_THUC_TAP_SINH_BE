using YC4_THUC_TAP_SINH_BE.Dto;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public interface IAuthInterface
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<LoginResponse> Register(RegisterRequest registerRequest);
    }
}
