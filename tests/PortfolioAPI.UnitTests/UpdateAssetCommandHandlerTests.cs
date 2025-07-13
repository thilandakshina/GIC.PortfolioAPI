using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.DTOs;
using PortfolioAPI.Application.Handlers;
using PortfolioAPI.Application.Interfaces;
using PortfolioAPI.Domain;

namespace PortfolioAPI.UnitTests
{
    public class UpdateAssetCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UpdateAssetCommandHandler _handler;
        private Asset _existingAsset;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateAssetCommandHandler(_unitOfWorkMock.Object);
            _existingAsset = new Asset { Id = Guid.NewGuid(), Name = "Old Name", CurrentPrice = 50 };
        }

        [Test]
        public async Task Handle_ShouldUpdateAsset_WhenNameIsUnique()
        {
            _unitOfWorkMock.Setup(u => u.Assets.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_existingAsset);
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Assets.UpdateAsync(It.IsAny<Asset>())).ReturnsAsync((Asset a) => a);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new UpdateAssetCommand { Id = _existingAsset.Id, Name = "New Name", CurrentPrice = 100 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(command.CurrentPrice, result.CurrentPrice);
        }

        [Test]
        public void Handle_ShouldThrow_WhenNameExists()
        {
            _unitOfWorkMock.Setup(u => u.Assets.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_existingAsset);
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(true);
            var command = new UpdateAssetCommand { Id = _existingAsset.Id, Name = "New Name", CurrentPrice = 100 };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
} 