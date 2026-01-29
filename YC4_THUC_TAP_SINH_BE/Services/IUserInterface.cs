using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public interface IUserInterface
    {
        // User CRUD
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int userId);
        Task<bool> ExistsAsync(string username);

        // Role Management
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<List<Role>> GetUserRoleObjectsAsync(int userId);
        Task<bool> HasRoleAsync(int userId, string roleName);
        Task<bool> AssignRoleAsync(int userId, int roleId);
        Task<bool> RemoveRoleAsync(int userId, int roleId);

        // Function/Permission Management
        Task<List<string>> GetUserFunctionsAsync(int userId);
        Task<List<Function>> GetUserFunctionObjectsAsync(int userId);
        Task<bool> HasFunctionAsync(int userId, string functionCode);
        Task<bool> AssignFunctionAsync(int userId, int functionId);
        Task<bool> RemoveFunctionAsync(int userId, int functionId);
    }
}
