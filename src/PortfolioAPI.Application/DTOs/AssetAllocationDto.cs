namespace PortfolioAPI.Application.DTOs
{
    public class AssetAllocationDto
    {
        public decimal TotalValue { get; set; }
        public List<AssetAllocationItemDto> Assets { get; set; } = new List<AssetAllocationItemDto>();
    }

    public class AssetAllocationItemDto
    {
        public Guid AssetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AllocationPercentage { get; set; }
        public decimal UnrealizedGainLoss { get; set; }
    }
} 