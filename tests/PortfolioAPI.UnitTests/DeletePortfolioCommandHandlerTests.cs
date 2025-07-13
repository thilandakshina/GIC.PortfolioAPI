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
    public class DeletePortfolioCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private DeletePortfolioCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeletePortfolioCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldDeletePortfolio_WhenPortfolioExists()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.Portfolios.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new DeletePortfolioCommand { Id = Guid.NewGuid() };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.IsTrue(result);
        }

        [Test]
        public void Handle_ShouldThrow_WhenPortfolioNotFound()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var command = new DeletePortfolioCommand { Id = Guid.NewGuid() };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
} 