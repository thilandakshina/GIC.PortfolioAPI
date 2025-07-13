using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Queries
{
    public class GetAllPortfoliosQuery : IRequest<List<PortfolioDto>>
    {
    }
} 