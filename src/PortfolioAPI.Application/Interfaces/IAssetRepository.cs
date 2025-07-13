using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Interfaces
{
    public interface IAssetRepository
    {
        Task<Asset?> GetByIdAsync(Guid id);
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset> AddAsync(Asset asset);
        Task<Asset> UpdateAsync(Asset asset);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByIdAsync(Guid id);
        Task<bool> IsAssetUsedInPortfoliosAsync(Guid assetId);
    }
} 