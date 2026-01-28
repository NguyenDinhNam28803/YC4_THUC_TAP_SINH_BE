namespace YC4_THUC_TAP_SINH_BE.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<User_Role> UserRoles { get; set; } = new List<User_Role>();
        public ICollection<User_Function> UserFunctions { get; set; } = new List<User_Function>();
    }
}
