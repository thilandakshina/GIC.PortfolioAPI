using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetPortfolioQueryHandler : IRequestHandler<GetPortfolioQuery, PortfolioDto>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAssetRepository _portfolioAssetRepository;
        private readonly IAssetRepository _assetRepository;

        public GetPortfolioQueryHandler(
            IPortfolioRepository portfolioRepository,
            IPortfolioAssetRepository portfolioAssetRepository,
            IAssetRepository assetRepository)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioAssetRepository = portfolioAssetRepository;
            _assetRepository = assetRepository;
        }

        public async Task<PortfolioDto> Handle(GetPortfolioQuery request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id);
            if (portfolio == null)
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.Id}' not found");
            }

            var portfolioAssets = await _portfolioAssetRepository.GetByPortfolioIdAsync(request.Id);
            decimal totalValue = 0;

            foreach (var portfolioAsset in portfolioAssets)
            {
                var asset = await _assetRepository.GetByIdAsync(portfolioAsset.AssetId);
                if (asset != null)
                {
                    totalValue += portfolioAsset.Quantity * asset.CurrentPrice;
                }
            }

            return new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                CreatedDate = portfolio.CreatedDate,
                TotalValue = totalValue
            };
        }
    }
} 