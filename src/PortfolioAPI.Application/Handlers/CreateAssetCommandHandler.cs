using MediatR;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;

namespace PortfolioAPI.Application.Handlers
{
    public class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, AssetDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAssetCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AssetDto> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Assets.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Asset name '{request.Name}' already exists");
            }

            var asset = new Asset
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CurrentPrice = request.CurrentPrice
            };

            var createdAsset = await _unitOfWork.Assets.AddAsync(asset);
            await _unitOfWork.SaveChangesAsync();

            return new AssetDto
            {
                Id = createdAsset.Id,
                Name = createdAsset.Name,
                CurrentPrice = createdAsset.CurrentPrice
            };
        }
    }
} 