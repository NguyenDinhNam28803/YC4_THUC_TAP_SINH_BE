using Azure.Core;
using BCrypt;
using Microsoft.EntityFrameworkCore;
using YC4_THUC_TAP_SINH_BE.Data;
using YC4_THUC_TAP_SINH_BE.Dto;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IUserInterface _userInterface;
        public AuthService(ApplicationDbContext context, IJwtService jwtService, IUserInterface userInterface)
        {
            _context = context;
            _jwtService = jwtService;
            _userInterface = userInterface;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.UserName || u.Email == loginRequest.UserName);
                if (user == null)
                {
                    var response = new LoginResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                    return response;
                }

                var checkPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);
                if (!checkPassword)
                {
                    var response = new LoginResponse
                    {
                        Success = false,
                        Message = "Invaid Password"
                    };
                    return response;
                }

                int expiryMinutes = 60;
                var roles = await _userInterface.GetUserRolesAsync(user.UserId);
                var permissions = await _userInterface.GetUserFunctionsAsync(user.UserId);

                var token = _jwtService.GenerateToken(user, roles, permissions);
                var loginResponse = new LoginResponse
                {
                    Success = true,
                    Token = token,
                    Expiration = DateTime.UtcNow.AddMinutes(expiryMinutes),
                    User = new UserInfo
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        FullName = user.Username,
                        Email = user.Email,
                        Roles = roles,
                        Permissions = permissions
                    },
                    Message = "Login successfuly"
                };
                return loginResponse;
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = ex.Message,
                }; 
            }
        }

        public async Task<LoginResponse> Register(RegisterRequest registerRequest)
        {
            try
            {
                var userExsit = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerRequest.Username);
                if (userExsit != null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Username exsit"
                    };
                }

                var existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
                if (existingUserWithEmail != null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email exsit"
                    };
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                var user = new User
                {
                    Username = registerRequest.Username,
                    PasswordHash = passwordHash,
                    FullName = registerRequest.FullName,
                    Email = registerRequest.Email,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                var createdUser = await _userInterface.CreateAsync(user);
                return new LoginResponse
                {
                    Success = true,
                    Message = "Register Successfuly"
                };
            }
            catch (Exception ex) 
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
