using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetPortfolioPerformanceQuery : IRequest<PortfolioPerformanceDto>
    {
        public Guid PortfolioId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
} 