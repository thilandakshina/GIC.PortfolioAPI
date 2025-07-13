namespace PortfolioAPI.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPortfolioRepository Portfolios { get; }
        IAssetRepository Assets { get; }
        ITransactionRepository Transactions { get; }
        IPortfolioAssetRepository PortfolioAssets { get; }
        Task<int> SaveChangesAsync();
    }
} 