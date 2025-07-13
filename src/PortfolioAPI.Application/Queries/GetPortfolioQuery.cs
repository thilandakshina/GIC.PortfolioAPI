using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetPortfolioQuery : IRequest<PortfolioDto>
    {
        public Guid Id { get; set; }
    }
} 