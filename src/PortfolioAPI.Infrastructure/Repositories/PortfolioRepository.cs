using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioAPI.Infrastructure.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly PortfolioDbContext _context;

        public PortfolioRepository(PortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio?> GetByIdAsync(Guid id)
        {
            return await _context.Portfolios
                .Include(p => p.PortfolioAssets)
                .ThenInclude(pa => pa.Asset)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Portfolio>> GetAllAsync()
        {
            return await _context.Portfolios
                .Include(p => p.PortfolioAssets)
                .ThenInclude(pa => pa.Asset)
                .ToListAsync();
        }

        public async Task<Portfolio> AddAsync(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> UpdateAsync(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null)
                return false;

            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Portfolios.AnyAsync(p => p.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _context.Portfolios.AnyAsync(p => p.Id == id);
        }
    }
} 