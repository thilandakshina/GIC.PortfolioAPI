namespace PortfolioAPI.Application.DTOs
{
    public class PortfolioPerformanceDto
    {
        public decimal TotalValue { get; set; }
        public decimal UnrealizedGainLoss { get; set; }
        public decimal RealizedGainLoss { get; set; }
        public decimal TotalGainLoss { get; set; }
        public List<PerformanceHistoryDto> PerformanceHistory { get; set; } = new List<PerformanceHistoryDto>();
    }

    public class PerformanceHistoryDto
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
} 