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
    public class CreateAssetCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CreateAssetCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateAssetCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateAsset_WhenNameIsUnique()
        {
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Assets.AddAsync(It.IsAny<Asset>())).ReturnsAsync((Asset a) => a);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new CreateAssetCommand { Name = "Test Asset", CurrentPrice = 100 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(command.CurrentPrice, result.CurrentPrice);
        }

        [Test]
        public void Handle_ShouldThrow_WhenNameExists()
        {
            _unitOfWorkMock.Setup(u => u.Assets.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(true);
            var command = new CreateAssetCommand { Name = "Test Asset", CurrentPrice = 100 };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
} 