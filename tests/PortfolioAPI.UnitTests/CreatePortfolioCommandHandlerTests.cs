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
    public class CreatePortfolioCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CreatePortfolioCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreatePortfolioCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldCreatePortfolio_WhenNameIsUnique()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Portfolios.AddAsync(It.IsAny<Portfolio>())).ReturnsAsync((Portfolio p) => p);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new CreatePortfolioCommand { Name = "Test Portfolio" };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(0, result.TotalValue);
        }

        [Test]
        public void Handle_ShouldThrow_WhenNameExists()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.ExistsByNameAsync(It.IsAny<string>())).ReturnsAsync(true);
            var command = new CreatePortfolioCommand { Name = "Test Portfolio" };
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
} 