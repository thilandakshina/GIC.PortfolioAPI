using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Commands
{
    public class CreateAssetCommand : IRequest<AssetDto>
    {
        public string Name { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
    }
} 