using System;

namespace PortfolioAPI.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; } = null!;

        public Guid AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
} 