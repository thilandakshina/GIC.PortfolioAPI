using MediatR;
using System;

namespace PortfolioAPI.Application.Commands
{
    public class DeleteAssetCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
} 