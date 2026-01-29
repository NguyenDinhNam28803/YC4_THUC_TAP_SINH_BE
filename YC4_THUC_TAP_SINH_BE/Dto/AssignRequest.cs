using System.ComponentModel.DataAnnotations;

namespace YC4_THUC_TAP_SINH_BE.Dto
{
    // Đăng ký quyền cho user
    public class AssignRoleRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }

    // Đăng ký chức năng cho user
    public class AssignFunctionRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int FunctionId { get; set; }
    }

    // Check permission (Chưa rõ lắm)
    public class CheckPermissionRequest
    {
        [Required]
        public string FunctionCode { get; set; }
    }
}
