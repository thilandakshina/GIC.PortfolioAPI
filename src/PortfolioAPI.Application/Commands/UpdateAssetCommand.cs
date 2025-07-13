using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Commands
{
    public class UpdateAssetCommand : IRequest<AssetDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
    }
} 