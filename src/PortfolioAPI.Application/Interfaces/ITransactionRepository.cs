using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetByPortfolioIdAsync(Guid portfolioId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Transaction> AddAsync(Transaction transaction);
        Task<int> GetAssetQuantityInPortfolioAsync(Guid portfolioId, Guid assetId);
    }
} 