using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<Portfolio?> GetByIdAsync(Guid id);
        Task<IEnumerable<Portfolio>> GetAllAsync();
        Task<Portfolio> AddAsync(Portfolio portfolio);
        Task<Portfolio> UpdateAsync(Portfolio portfolio);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByIdAsync(Guid id);
    }
} 