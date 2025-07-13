using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetAssetQuery : IRequest<AssetDto>
    {
        public Guid Id { get; set; }
    }
} 