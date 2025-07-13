using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.Interfaces;

namespace PortfolioAPI.Application.Handlers
{
    public class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAssetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Assets.ExistsByIdAsync(request.Id))
            {
                throw new InvalidOperationException($"Asset with ID '{request.Id}' not found");
            }

            if (await _unitOfWork.Assets.IsAssetUsedInPortfoliosAsync(request.Id))
            {
                throw new InvalidOperationException("Cannot delete asset that is used in portfolios");
            }

            var result = await _unitOfWork.Assets.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
} 