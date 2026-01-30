using Microsoft.EntityFrameworkCore;
using YC4_THUC_TAP_SINH_BE.Data;
using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public class FunctionService : IFunctionInterface
    {
        private readonly ApplicationDbContext _context;
        public FunctionService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<Function>> GetAllAsync()
        {
            return await _context.Functions
                .Include(f => f.UserFunctions)
                    .ThenInclude(f => f.User)
                .Include(r => r.RoleFunctions)
                    .ThenInclude(r => r.Role)
                .ToListAsync();
        }

        public async Task<Function> GetByIdAsync(int functionId)
        {
            var function = await _context.Functions.FindAsync(functionId);
            return function;
        }

        public async Task<Function> GetByCodeAsync(string functionCode)
        {
            var function = await _context.Functions
                .FirstOrDefaultAsync(f => f.FunctionCode == functionCode);
            return function;
        }

        public async Task<Function> CreateAsync(Function function)
        {
            _context.Functions.Add(function);
            await _context.SaveChangesAsync();
            return function;
        }

        public async Task<Function> UpdateAsync(Function function)
        {
            _context.Functions.Update(function);
            await _context.SaveChangesAsync();
            return function;
        }

        public async Task<bool> DeleteAsync(int functionId)
        {
            var function = await _context.Functions.FindAsync(functionId);
            if (function == null) return false;

            _context.Functions.Remove(function);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
