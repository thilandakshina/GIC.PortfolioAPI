using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAPI.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly PortfolioDbContext _context;

        public AssetRepository(PortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            return await _context.Assets
                .Include(a => a.PortfolioAssets)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets
                .Include(a => a.PortfolioAssets)
                .ToListAsync();
        }

        public async Task<Asset> AddAsync(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<Asset> UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
                return false;

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Assets.AnyAsync(a => a.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _context.Assets.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> IsAssetUsedInPortfoliosAsync(Guid assetId)
        {
            return await _context.PortfolioAssets.AnyAsync(pa => pa.AssetId == assetId);
        }
    }
} 