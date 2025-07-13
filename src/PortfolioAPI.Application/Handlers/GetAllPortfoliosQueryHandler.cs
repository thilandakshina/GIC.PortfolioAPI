using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetAllPortfoliosQueryHandler : IRequestHandler<GetAllPortfoliosQuery, List<PortfolioDto>>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAssetRepository _portfolioAssetRepository;
        private readonly IAssetRepository _assetRepository;

        public GetAllPortfoliosQueryHandler(
            IPortfolioRepository portfolioRepository,
            IPortfolioAssetRepository portfolioAssetRepository,
            IAssetRepository assetRepository)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioAssetRepository = portfolioAssetRepository;
            _assetRepository = assetRepository;
        }

        public async Task<List<PortfolioDto>> Handle(GetAllPortfoliosQuery request, CancellationToken cancellationToken)
        {
            var portfolios = await _portfolioRepository.GetAllAsync();
            var result = new List<PortfolioDto>();

            foreach (var portfolio in portfolios)
            {
                var portfolioAssets = await _portfolioAssetRepository.GetByPortfolioIdAsync(portfolio.Id);
                decimal totalValue = 0;

                foreach (var portfolioAsset in portfolioAssets)
                {
                    var asset = await _assetRepository.GetByIdAsync(portfolioAsset.AssetId);
                    if (asset != null)
                    {
                        totalValue += portfolioAsset.Quantity * asset.CurrentPrice;
                    }
                }

                result.Add(new PortfolioDto
                {
                    Id = portfolio.Id,
                    Name = portfolio.Name,
                    CreatedDate = portfolio.CreatedDate,
                    TotalValue = totalValue
                });
            }

            return result;
        }
    }
} 