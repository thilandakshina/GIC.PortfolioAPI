using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetTransactionHistoryQuery : IRequest<List<TransactionDto>>
    {
        public Guid PortfolioId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
} 