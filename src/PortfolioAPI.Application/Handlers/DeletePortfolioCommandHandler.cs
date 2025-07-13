using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.Interfaces;

namespace PortfolioAPI.Application.Handlers
{
    public class DeletePortfolioCommandHandler : IRequestHandler<DeletePortfolioCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePortfolioCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Portfolios.ExistsByIdAsync(request.Id))
            {
                throw new InvalidOperationException($"Portfolio with ID '{request.Id}' not found");
            }

            var result = await _unitOfWork.Portfolios.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
} 