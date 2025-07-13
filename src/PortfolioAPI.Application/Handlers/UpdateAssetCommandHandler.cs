using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;

namespace PortfolioAPI.Application.Handlers
{
    public class UpdateAssetCommandHandler : IRequestHandler<UpdateAssetCommand, AssetDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAssetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AssetDto> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _unitOfWork.Assets.GetByIdAsync(request.Id);
            if (asset == null)
            {
                throw new InvalidOperationException($"Asset with ID '{request.Id}' not found");
            }

            if (request.Name != asset.Name && await _unitOfWork.Assets.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Asset name '{request.Name}' already exists");
            }

            asset.Name = request.Name;
            asset.CurrentPrice = request.CurrentPrice;
            var updatedAsset = await _unitOfWork.Assets.UpdateAsync(asset);
            await _unitOfWork.SaveChangesAsync();

            return new AssetDto
            {
                Id = updatedAsset.Id,
                Name = updatedAsset.Name,
                CurrentPrice = updatedAsset.CurrentPrice
            };
        }
    }
} 