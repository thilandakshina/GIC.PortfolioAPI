using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid PortfolioId { get; set; }
        public Guid AssetId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
} 