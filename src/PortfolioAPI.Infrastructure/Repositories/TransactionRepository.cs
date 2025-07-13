using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioAPI.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PortfolioDbContext _context;

        public TransactionRepository(PortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(Guid id)
        {
            return await _context.Transactions
                .Include(t => t.Portfolio)
                .Include(t => t.Asset)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetByPortfolioIdAsync(Guid portfolioId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Transactions
                .Include(t => t.Portfolio)
                .Include(t => t.Asset)
                .Where(t => t.PortfolioId == portfolioId);

            if (startDate.HasValue)
                query = query.Where(t => t.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.Date <= endDate.Value);

            return await query.OrderByDescending(t => t.Date).ToListAsync();
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<int> GetAssetQuantityInPortfolioAsync(Guid portfolioId, Guid assetId)
        {
            var buyQuantity = await _context.Transactions
                .Where(t => t.PortfolioId == portfolioId && t.AssetId == assetId && t.TransactionType == TransactionType.Buy)
                .SumAsync(t => t.Quantity);

            var sellQuantity = await _context.Transactions
                .Where(t => t.PortfolioId == portfolioId && t.AssetId == assetId && t.TransactionType == TransactionType.Sell)
                .SumAsync(t => t.Quantity);

            return buyQuantity - sellQuantity;
        }
    }
} 