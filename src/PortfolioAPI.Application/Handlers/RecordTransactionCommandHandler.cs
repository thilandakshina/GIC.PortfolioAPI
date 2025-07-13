using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Handlers
{
    public class RecordTransactionCommandHandler : IRequestHandler<RecordTransactionCommand, TransactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecordTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionDto> Handle(RecordTransactionCommand request, CancellationToken cancellationToken)
        {
            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(request.PortfolioId);
            if (portfolio == null)
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.PortfolioId}' not found");
            }

            var asset = await _unitOfWork.Assets.GetByIdAsync(request.AssetId);
            if (asset == null)
            {
                throw new InvalidOperationException($"Asset with ID '{request.AssetId}' not found");
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                PortfolioId = request.PortfolioId,
                AssetId = request.AssetId,
                TransactionType = request.TransactionType,
                Quantity = request.Quantity,
                Price = request.Price,
                Date = request.TransactionDate
            };

            var createdTransaction = await _unitOfWork.Transactions.AddAsync(transaction);

            await UpdatePortfolioAsset(request);

            await _unitOfWork.SaveChangesAsync();

            return new TransactionDto
            {
                Id = createdTransaction.Id,
                PortfolioId = createdTransaction.PortfolioId,
                AssetId = createdTransaction.AssetId,
                TransactionType = createdTransaction.TransactionType,
                Quantity = createdTransaction.Quantity,
                Price = createdTransaction.Price,
                Date = createdTransaction.Date
            };
        }

        private async Task UpdatePortfolioAsset(RecordTransactionCommand request)
        {
            var portfolioAsset = await _unitOfWork.PortfolioAssets.GetAsync(request.PortfolioId, request.AssetId);

            if (request.TransactionType == TransactionType.Buy)
            {
                if (portfolioAsset == null)
                {
                    portfolioAsset = new PortfolioAsset
                    {
                        PortfolioId = request.PortfolioId,
                        AssetId = request.AssetId,
                        Quantity = request.Quantity,
                        AverageCostBasis = request.Price
                    };
                    await _unitOfWork.PortfolioAssets.AddAsync(portfolioAsset);
                }
                else
                {
                    var totalCost = (portfolioAsset.Quantity * portfolioAsset.AverageCostBasis) + (request.Quantity * request.Price);
                    var totalQuantity = portfolioAsset.Quantity + request.Quantity;
                    portfolioAsset.Quantity = totalQuantity;
                    portfolioAsset.AverageCostBasis = totalCost / totalQuantity;
                    await _unitOfWork.PortfolioAssets.UpdateAsync(portfolioAsset);
                }
            }
            else
            {
                if (portfolioAsset == null || portfolioAsset.Quantity < request.Quantity)
                {
                    throw new InvalidOperationException("Cannot sell more shares than owned");
                }

                portfolioAsset.Quantity -= request.Quantity;
                if (portfolioAsset.Quantity == 0)
                {
                    await _unitOfWork.PortfolioAssets.DeleteAsync(request.PortfolioId, request.AssetId);
                }
                else
                {
                    await _unitOfWork.PortfolioAssets.UpdateAsync(portfolioAsset);
                }
            }
        }
    }
} 