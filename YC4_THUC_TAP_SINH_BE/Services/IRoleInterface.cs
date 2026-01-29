using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public interface IRoleInterface
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int roleId);
        Task<Role> GetByNameAsync(string roleName);
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task<bool> DeleteAsync(int roleId);
    }
}
