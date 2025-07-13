using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;

namespace PortfolioAPI.Application.Handlers
{
    public class UpdatePortfolioCommandHandler : IRequestHandler<UpdatePortfolioCommand, PortfolioDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePortfolioCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PortfolioDto> Handle(UpdatePortfolioCommand request, CancellationToken cancellationToken)
        {
            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(request.Id);
            if (portfolio == null)
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.Id}' not found");
            }

            if (request.Name != portfolio.Name && await _unitOfWork.Portfolios.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Portfolio name '{request.Name}' already exists");
            }

            portfolio.Name = request.Name;
            var updatedPortfolio = await _unitOfWork.Portfolios.UpdateAsync(portfolio);
            await _unitOfWork.SaveChangesAsync();

            return new PortfolioDto
            {
                Id = updatedPortfolio.Id,
                Name = updatedPortfolio.Name,
                CreatedDate = updatedPortfolio.CreatedDate,
                TotalValue = 0
            };
        }
    }
} 