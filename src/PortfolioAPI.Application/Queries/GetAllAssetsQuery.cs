using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetAllAssetsQuery : IRequest<List<AssetDto>>
    {
    }
} 