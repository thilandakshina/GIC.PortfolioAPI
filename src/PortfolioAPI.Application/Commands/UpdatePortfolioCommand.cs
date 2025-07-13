using MediatR;
using PortfolioAPI.Application.DTOs;

namespace PortfolioAPI.Application.Commands
{
    public class UpdatePortfolioCommand : IRequest<PortfolioDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
} 