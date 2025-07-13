using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAPI.Infrastructure.Repositories
{
    public class PortfolioAssetRepository : IPortfolioAssetRepository
    {
        private readonly PortfolioDbContext _context;

        public PortfolioAssetRepository(PortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<PortfolioAsset?> GetAsync(Guid portfolioId, Guid assetId)
        {
            return await _context.PortfolioAssets
                .Include(pa => pa.Portfolio)
                .Include(pa => pa.Asset)
                .FirstOrDefaultAsync(pa => pa.PortfolioId == portfolioId && pa.AssetId == assetId);
        }

        public async Task<IEnumerable<PortfolioAsset>> GetByPortfolioIdAsync(Guid portfolioId)
        {
            return await _context.PortfolioAssets
                .Include(pa => pa.Portfolio)
                .Include(pa => pa.Asset)
                .Where(pa => pa.PortfolioId == portfolioId)
                .ToListAsync();
        }

        public async Task<PortfolioAsset> AddAsync(PortfolioAsset portfolioAsset)
        {
            _context.PortfolioAssets.Add(portfolioAsset);
            await _context.SaveChangesAsync();
            return portfolioAsset;
        }

        public async Task<PortfolioAsset> UpdateAsync(PortfolioAsset portfolioAsset)
        {
            _context.PortfolioAssets.Update(portfolioAsset);
            await _context.SaveChangesAsync();
            return portfolioAsset;
        }

        public async Task<bool> DeleteAsync(Guid portfolioId, Guid assetId)
        {
            var portfolioAsset = await _context.PortfolioAssets
                .FirstOrDefaultAsync(pa => pa.PortfolioId == portfolioId && pa.AssetId == assetId);

            if (portfolioAsset == null)
                return false;

            _context.PortfolioAssets.Remove(portfolioAsset);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid portfolioId, Guid assetId)
        {
            return await _context.PortfolioAssets
                .AnyAsync(pa => pa.PortfolioId == portfolioId && pa.AssetId == assetId);
        }
    }
} 