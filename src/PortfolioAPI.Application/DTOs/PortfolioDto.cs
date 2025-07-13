namespace PortfolioAPI.Application.DTOs
{
    public class PortfolioDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public decimal TotalValue { get; set; }
    }
} 