using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Interfaces
{
    public interface IPortfolioAssetRepository
    {
        Task<PortfolioAsset?> GetAsync(Guid portfolioId, Guid assetId);
        Task<IEnumerable<PortfolioAsset>> GetByPortfolioIdAsync(Guid portfolioId);
        Task<PortfolioAsset> AddAsync(PortfolioAsset portfolioAsset);
        Task<PortfolioAsset> UpdateAsync(PortfolioAsset portfolioAsset);
        Task<bool> DeleteAsync(Guid portfolioId, Guid assetId);
        Task<bool> ExistsAsync(Guid portfolioId, Guid assetId);
    }
} 