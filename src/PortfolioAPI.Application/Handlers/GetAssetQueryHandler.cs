using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetAssetQueryHandler : IRequestHandler<GetAssetQuery, AssetDto>
    {
        private readonly IAssetRepository _assetRepository;

        public GetAssetQueryHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<AssetDto> Handle(GetAssetQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.Id);
            if (asset == null)
            {
                throw new InvalidOperationException($"Asset with ID '{request.Id}' not found");
            }

            return new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                CurrentPrice = asset.CurrentPrice
            };
        }
    }
} 