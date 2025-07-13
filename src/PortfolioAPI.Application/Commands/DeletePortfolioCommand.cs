using MediatR;
using System;

namespace PortfolioAPI.Application.Commands
{
    public class DeletePortfolioCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
} 