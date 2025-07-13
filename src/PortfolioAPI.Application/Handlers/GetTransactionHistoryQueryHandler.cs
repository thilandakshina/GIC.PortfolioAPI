using MediatR;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Application.Queries;

namespace PortfolioAPI.Application.Handlers
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, List<TransactionDto>>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionHistoryQueryHandler(
            IPortfolioRepository portfolioRepository,
            ITransactionRepository transactionRepository)
        {
            _portfolioRepository = portfolioRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionDto>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.PortfolioId);
            if (portfolio == null)
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.PortfolioId}' not found");
            }

            var transactions = await _transactionRepository.GetByPortfolioIdAsync(request.PortfolioId, request.StartDate, request.EndDate);

            return transactions.Select(transaction => new TransactionDto
            {
                Id = transaction.Id,
                PortfolioId = transaction.PortfolioId,
                AssetId = transaction.AssetId,
                TransactionType = transaction.TransactionType,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                Date = transaction.Date
            }).ToList();
        }
    }
} 