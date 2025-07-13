using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetAllAssetsQueryHandler : IRequestHandler<GetAllAssetsQuery, List<AssetDto>>
    {
        private readonly IAssetRepository _assetRepository;

        public GetAllAssetsQueryHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<List<AssetDto>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAllAsync();
            return assets.Select(asset => new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                CurrentPrice = asset.CurrentPrice
            }).ToList();
        }
    }
} 