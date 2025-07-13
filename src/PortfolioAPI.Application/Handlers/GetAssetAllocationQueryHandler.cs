using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetAssetAllocationQueryHandler : IRequestHandler<GetAssetAllocationQuery, AssetAllocationDto>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAssetRepository _portfolioAssetRepository;
        private readonly IAssetRepository _assetRepository;

        public GetAssetAllocationQueryHandler(
            IPortfolioRepository portfolioRepository,
            IPortfolioAssetRepository portfolioAssetRepository,
            IAssetRepository assetRepository)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioAssetRepository = portfolioAssetRepository;
            _assetRepository = assetRepository;
        }

        public async Task<AssetAllocationDto> Handle(GetAssetAllocationQuery request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.PortfolioId);
            if (portfolio == null)
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.PortfolioId}' not found");
            }

            var portfolioAssets = await _portfolioAssetRepository.GetByPortfolioIdAsync(request.PortfolioId);
            var result = new AssetAllocationDto();
            var assetAllocations = new List<AssetAllocationItemDto>();

            decimal totalValue = 0;

            foreach (var portfolioAsset in portfolioAssets)
            {
                var asset = await _assetRepository.GetByIdAsync(portfolioAsset.AssetId);
                if (asset != null)
                {
                    var assetValue = portfolioAsset.Quantity * asset.CurrentPrice;
                    var unrealizedGainLoss = assetValue - (portfolioAsset.Quantity * portfolioAsset.AverageCostBasis);
                    totalValue += assetValue;

                    assetAllocations.Add(new AssetAllocationItemDto
                    {
                        AssetId = asset.Id,
                        Name = asset.Name,
                        Quantity = portfolioAsset.Quantity,
                        CurrentPrice = asset.CurrentPrice,
                        TotalValue = assetValue,
                        AllocationPercentage = 0,
                        UnrealizedGainLoss = unrealizedGainLoss
                    });
                }
            }

            foreach (var allocation in assetAllocations)
            {
                allocation.AllocationPercentage = totalValue > 0 ? (allocation.TotalValue / totalValue) * 100 : 0;
            }

            result.TotalValue = totalValue;
            result.Assets = assetAllocations;

            return result;
        }
    }
} 