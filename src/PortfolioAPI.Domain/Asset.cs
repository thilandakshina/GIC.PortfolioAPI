using System;
using System.Collections.Generic;

namespace PortfolioAPI.Domain
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }

        public ICollection<PortfolioAsset> PortfolioAssets { get; set; } = new List<PortfolioAsset>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
} 