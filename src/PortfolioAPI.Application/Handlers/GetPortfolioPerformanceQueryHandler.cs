using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetPortfolioPerformanceQueryHandler : IRequestHandler<GetPortfolioPerformanceQuery, PortfolioPerformanceDto>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAssetRepository _portfolioAssetRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ITransactionRepository _transactionRepository;

        public GetPortfolioPerformanceQueryHandler(
            IPortfolioRepository portfolioRepository,
            IPortfolioAssetRepository portfolioAssetRepository,
            IAssetRepository assetRepository,
            ITransactionRepository transactionRepository)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioAssetRepository = portfolioAssetRepository;
            _assetRepository = assetRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<PortfolioPerformanceDto> Handle(GetPortfolioPerformanceQuery request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.PortfolioId);
            if (portfolio == null)
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.PortfolioId}' not found");
            }

            var portfolioAssets = await _portfolioAssetRepository.GetByPortfolioIdAsync(request.PortfolioId);
            var transactions = await _transactionRepository.GetByPortfolioIdAsync(request.PortfolioId, request.StartDate, request.EndDate);

            decimal totalValue = 0;
            decimal unrealizedGainLoss = 0;
            decimal realizedGainLoss = 0;

            foreach (var portfolioAsset in portfolioAssets)
            {
                var asset = await _assetRepository.GetByIdAsync(portfolioAsset.AssetId);
                if (asset != null)
                {
                    var currentValue = portfolioAsset.Quantity * asset.CurrentPrice;
                    var costBasis = portfolioAsset.Quantity * portfolioAsset.AverageCostBasis;
                    totalValue += currentValue;
                    unrealizedGainLoss += currentValue - costBasis;
                }
            }

            var sellTransactions = transactions.Where(t => t.TransactionType == Domain.TransactionType.Sell);
            foreach (var transaction in sellTransactions)
            {
                var portfolioAsset = portfolioAssets.FirstOrDefault(pa => pa.AssetId == transaction.AssetId);
                if (portfolioAsset != null)
                {
                    var costBasis = transaction.Quantity * portfolioAsset.AverageCostBasis;
                    var saleValue = transaction.Quantity * transaction.Price;
                    realizedGainLoss += saleValue - costBasis;
                }
            }

            return new PortfolioPerformanceDto
            {
                TotalValue = totalValue,
                UnrealizedGainLoss = unrealizedGainLoss,
                RealizedGainLoss = realizedGainLoss,
                TotalGainLoss = unrealizedGainLoss + realizedGainLoss,
                PerformanceHistory = new List<PerformanceHistoryDto>()
            };
        }
    }
} 