namespace YC4_THUC_TAP_SINH_BE.Models
{
    public class Role_Function
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public DateTime AssignedAt { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public Function Function { get; set; }
    }
}
