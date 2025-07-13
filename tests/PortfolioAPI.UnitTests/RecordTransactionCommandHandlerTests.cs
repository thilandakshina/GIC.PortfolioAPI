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
    public class RecordTransactionCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private RecordTransactionCommandHandler _handler;
        private Portfolio _portfolio;
        private Asset _asset;
        private PortfolioAsset _portfolioAsset;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RecordTransactionCommandHandler(_unitOfWorkMock.Object);
            _portfolio = new Portfolio { Id = Guid.NewGuid(), Name = "Test Portfolio", CreatedDate = DateTime.UtcNow };
            _asset = new Asset { Id = Guid.NewGuid(), Name = "Test Asset", CurrentPrice = 100 };
            _portfolioAsset = new PortfolioAsset { PortfolioId = _portfolio.Id, AssetId = _asset.Id, Quantity = 10, AverageCostBasis = 100 };
        }

        [Test]
        public async Task Handle_ShouldRecordBuyTransaction()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_portfolio);
            _unitOfWorkMock.Setup(u => u.Assets.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_asset);
            _unitOfWorkMock.Setup(u => u.Transactions.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction t) => t);
            _unitOfWorkMock.Setup(u => u.PortfolioAssets.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((PortfolioAsset)null);
            _unitOfWorkMock.Setup(u => u.PortfolioAssets.AddAsync(It.IsAny<PortfolioAsset>())).ReturnsAsync((PortfolioAsset pa) => pa);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new RecordTransactionCommand { PortfolioId = _portfolio.Id, AssetId = _asset.Id, TransactionType = TransactionType.Buy, Quantity = 5, Price = 100, TransactionDate = DateTime.UtcNow };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(command.Quantity, result.Quantity);
            Assert.AreEqual(command.Price, result.Price);
        }

        [Test]
        public async Task Handle_ShouldRecordSellTransaction()
        {
            _unitOfWorkMock.Setup(u => u.Portfolios.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_portfolio);
            _unitOfWorkMock.Setup(u => u.Assets.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_asset);
            _unitOfWorkMock.Setup(u => u.Transactions.AddAsync(It.IsAny<Transaction>())).ReturnsAsync((Transaction t) => t);
            _unitOfWorkMock.Setup(u => u.PortfolioAssets.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(_portfolioAsset);
            _unitOfWorkMock.Setup(u => u.PortfolioAssets.UpdateAsync(It.IsAny<PortfolioAsset>())).ReturnsAsync((PortfolioAsset pa) => pa);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new RecordTransactionCommand { PortfolioId = _portfolio.Id, AssetId = _asset.Id, TransactionType = TransactionType.Sell, Quantity = 5, Price = 100, TransactionDate = DateTime.UtcNow };
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(command.Quantity, result.Quantity);
            Assert.AreEqual(command.Price, result.Price);
        }
    }
} 