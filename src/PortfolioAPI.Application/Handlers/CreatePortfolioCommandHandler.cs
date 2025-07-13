using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Handlers
{
    public class CreatePortfolioCommandHandler : IRequestHandler<CreatePortfolioCommand, PortfolioDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePortfolioCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PortfolioDto> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Portfolios.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Portfolio name '{request.Name}' already exists");
            }

            var portfolio = new Portfolio
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CreatedDate = DateTime.UtcNow
            };

            var createdPortfolio = await _unitOfWork.Portfolios.AddAsync(portfolio);
            await _unitOfWork.SaveChangesAsync();

            return new PortfolioDto
            {
                Id = createdPortfolio.Id,
                Name = createdPortfolio.Name,
                CreatedDate = createdPortfolio.CreatedDate,
                TotalValue = 0
            };
        }
    }
} 