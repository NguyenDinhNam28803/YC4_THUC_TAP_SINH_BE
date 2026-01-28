using System.ComponentModel.DataAnnotations;

namespace YC4_THUC_TAP_SINH_BE.Dto
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserInfo User { get; set; }
        public string Message { get; set; }
    }

    public class UserInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
