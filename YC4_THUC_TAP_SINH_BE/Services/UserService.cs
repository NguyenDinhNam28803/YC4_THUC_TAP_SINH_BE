using Microsoft.EntityFrameworkCore;
using YC4_THUC_TAP_SINH_BE.Data;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public class UserService : IUserInterface
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext applicationDbContext) 
        { 
            _context = applicationDbContext;
        }

        // Management User
        public async Task<User> GetByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.UserFunctions)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.UserFunctions)
                .FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserFunctions)
                    .ThenInclude(u => u.Function)
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        // Management role
        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();

            return roles;
        }

        public async Task<List<Role>> GetUserRoleObjectsAsync(int userId)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role)
                .ToListAsync();

            return roles;
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.Role.RoleName == roleName);
        }

        public async Task<bool> AssignRoleAsync(int userId, int roleId)
        {
            // Check if already exists
            var exists = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (exists) return false;

            var userRole = new User_Role
            {
                UserId = userId,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null) return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        // Management Function
        public async Task<List<string>> GetUserFunctionsAsync(int userId)
        {
            // Functions từ User_Function (quyền trực tiếp)
            var directFunctions = await _context.UserFunctions
                .Where(uf => uf.UserId == userId)
                .Include(uf => uf.Function)
                .Select(uf => uf.Function.FunctionCode)
                .ToListAsync();

            // Functions từ Role (quyền thông qua role)
            var roleFunctions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RoleFunctions)
                .Select(rf => rf.Function.FunctionCode)
                .ToListAsync();

            // Gộp lại và loại bỏ trùng
            var allFunctions = directFunctions.Union(roleFunctions).Distinct().ToList();

            return allFunctions;
        }

        public async Task<List<Function>> GetUserFunctionObjectsAsync(int userId)
        {
            // Functions từ User_Function (quyền trực tiếp)
            var directFunctions = await _context.UserFunctions
                .Where(uf => uf.UserId == userId)
                .Include(uf => uf.Function)
                .Select(uf => uf.Function)
                .ToListAsync();

            // Functions từ Role (quyền thông qua role)
            var roleFunctions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RoleFunctions)
                .Select(rf => rf.Function)
                .ToListAsync();

            // Gộp lại và loại bỏ trùng (theo FunctionId)
            var allFunctions = directFunctions
                .Union(roleFunctions)
                .GroupBy(f => f.FunctionId)
                .Select(g => g.First())
                .ToList();

            return allFunctions;
        }

        public async Task<bool> HasFunctionAsync(int userId, string functionCode)
        {
            // Kiểm tra quyền trực tiếp
            var hasDirectFunction = await _context.UserFunctions
                .AnyAsync(uf => uf.UserId == userId && uf.Function.FunctionCode == functionCode);

            if (hasDirectFunction)
                return true;

            // Kiểm tra quyền thông qua role
            var hasRoleFunction = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .AnyAsync(ur => ur.Role.RoleFunctions
                    .Any(rf => rf.Function.FunctionCode == functionCode));

            return hasRoleFunction;
        }

        public async Task<bool> AssignFunctionAsync(int userId, int functionId)
        {
            // Check if already exists
            var exists = await _context.UserFunctions
                .AnyAsync(uf => uf.UserId == userId && uf.FunctionId == functionId);

            if (exists) return false;

            var userFunction = new User_Function
            {
                UserId = userId,
                FunctionId = functionId,
                AssignedAt = DateTime.UtcNow
            };

            _context.UserFunctions.Add(userFunction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFunctionAsync(int userId, int functionId)
        {
            var userFunction = await _context.UserFunctions
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FunctionId == functionId);

            if (userFunction == null) return false;

            _context.UserFunctions.Remove(userFunction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
