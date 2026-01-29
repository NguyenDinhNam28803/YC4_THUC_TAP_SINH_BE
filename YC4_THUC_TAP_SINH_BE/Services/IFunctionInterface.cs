using YC4_THUC_TAP_SINH_BE.Models;

namespace YC4_THUC_TAP_SINH_BE.Services
{
    public interface IFunctionInterface
    {
        Task<List<Function>> GetAllAsync();
        Task<Function> GetByIdAsync(int functionId);
        Task<Function> GetByCodeAsync(string functionCode);
        Task<Function> CreateAsync(Function function);
        Task<Function> UpdateAsync(Function function);
        Task<bool> DeleteAsync(int functionId);
    }
}
