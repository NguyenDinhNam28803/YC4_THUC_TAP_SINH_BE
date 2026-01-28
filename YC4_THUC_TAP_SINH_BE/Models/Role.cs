using Microsoft.VisualBasic;

namespace YC4_THUC_TAP_SINH_BE.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<User_Role> UserRoles { get; set; } = new List<User_Role>();
        public ICollection<Role_Function> RoleFunctions { get; set; } = new List<Role_Function>();
    }
}
