using System;

namespace PortfolioAPI.Domain
{
    public class PortfolioAsset
    {
        public Guid PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; } = null!;

        public Guid AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal AverageCostBasis { get; set; }
    }
} 