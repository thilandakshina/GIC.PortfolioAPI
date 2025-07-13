using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PortfolioAPI.Application.Commands;
using PortfolioAPI.Application.Handlers;
using PortfolioAPI.Application.Interfaces;

namespace PortfolioAPI.UnitTests
{
    public class DeleteAssetCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private DeleteAssetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteAssetCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldDeleteAsset_WhenAssetExistsAndNotUsed()
        {
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Assets.IsAssetUsedInPortfoliosAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Assets.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new DeleteAssetCommand { Id = Guid.NewGuid() };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.IsTrue(result);
        }

        [Test]
        public void Handle_ShouldThrow_WhenAssetNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var command = new DeleteAssetCommand { Id = Guid.NewGuid() };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_ShouldThrow_WhenAssetUsedInPortfolio()
        {
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Assets.IsAssetUsedInPortfoliosAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            var command = new DeleteAssetCommand { Id = Guid.NewGuid() };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
} 