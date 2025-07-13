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
    public class UpdatePortfolioCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UpdatePortfolioCommandHandler _handler;
        private Portfolio _existingPortfolio;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdatePortfolioCommandHandler(_unitOfWorkMock.Object);
            _existingPortfolio = new Portfolio { Id = Guid.NewGuid(), Name = "Old Name", CreatedDate = DateTime.UtcNow };
        }

        [Test]
        public async Task Handle_ShouldUpdatePortfolio_WhenNameIsUnique()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_existingPortfolio);
            _unitOfWorkMock.Setup(u => u.Portfolios.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Portfolios.UpdateAsync(It.IsAny<Portfolio>())).ReturnsAsync((Portfolio p) => p);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new UpdatePortfolioCommand { Id = _existingPortfolio.Id, Name = "New Name" };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.Name, result.Name);
        }

        [Test]
        public void Handle_ShouldThrow_WhenNameExists()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_existingPortfolio);
            _unitOfWorkMock.Setup(u => u.Portfolios.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(true);
            var command = new UpdatePortfolioCommand { Id = _existingPortfolio.Id, Name = "New Name" };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
} 