using PortfolioAPI.Application.Interfaces;
using System.Threading.Tasks;

namespace PortfolioAPI.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PortfolioDbContext _context;
        public IPortfolioRepository Portfolios { get; }
        public IAssetRepository Assets { get; }
        public ITransactionRepository Transactions { get; }
        public IPortfolioAssetRepository PortfolioAssets { get; }

        public UnitOfWork(
            PortfolioDbContext context,
            IPortfolioRepository portfolios,
            IAssetRepository assets,
            ITransactionRepository transactions,
            IPortfolioAssetRepository portfolioAssets)
        {
            _context = context;
            Portfolios = portfolios;
            Assets = assets;
            Transactions = transactions;
            PortfolioAssets = portfolioAssets;
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
} 