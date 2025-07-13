using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Commands
{
    public class CreatePortfolioCommand : IRequest<PortfolioDto>
    {
        public string Name { get; set; } = string.Empty;
    }
} 