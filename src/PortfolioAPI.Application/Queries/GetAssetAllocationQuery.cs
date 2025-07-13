using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetAssetAllocationQuery : IRequest<AssetAllocationDto>
    {
        public Guid PortfolioId { get; set; }
    }
} 