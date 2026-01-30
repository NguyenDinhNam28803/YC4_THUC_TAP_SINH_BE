using System.Text.Json.Serialization;

namespace YC4_THUC_TAP_SINH_BE.Models
{
    public class User_Role
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        public int? AssignedBy { get; set; }

        [JsonIgnore]
        // Navigation properties
        public User User { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }
    }
}
