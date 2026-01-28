namespace YC4_THUC_TAP_SINH_BE.Models
{
    public class Function
    {
        public int FunctionId { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<User_Function> UserFunctions { get; set; } = new List<User_Function>();
        public ICollection<Role_Function> RoleFunctions { get; set; } = new List<Role_Function>();
    }
}
