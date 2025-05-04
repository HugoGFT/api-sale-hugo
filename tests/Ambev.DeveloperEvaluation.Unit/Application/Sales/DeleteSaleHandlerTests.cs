using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class DeleteSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleItemRepository> _saleItemRepositoryMock;
        private readonly DeleteSaleHandler _handler;
        private readonly Faker _faker;

        public DeleteSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
            _handler = new DeleteSaleHandler(_saleRepositoryMock.Object, _saleItemRepositoryMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldDeleteSaleAndItems_WhenSaleExists()
        {
            // Arrange
            var saleId = _faker.Random.Int(1, 1000);
            var saleItems = new List<SaleItem>
            {
                new SaleItem { Id = _faker.Random.Int(1, 1000), SaleId = saleId },
                new SaleItem { Id = _faker.Random.Int(1, 1000), SaleId = saleId }
            };

            _saleRepositoryMock
                .Setup(repo => repo.DeleteAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _saleItemRepositoryMock
                .Setup(repo => repo.GetBySaleIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleItems);

            _saleItemRepositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new DeleteSaleCommand(saleId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _saleRepositoryMock.Verify(repo => repo.DeleteAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(repo => repo.GetBySaleIdAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(saleItems.Count));
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenSaleDoesNotExist()
        {
            // Arrange
            var saleId = _faker.Random.Int(1, 1000);

            _saleRepositoryMock
                .Setup(repo => repo.DeleteAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new DeleteSaleCommand(saleId);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {saleId} not found");
            _saleRepositoryMock.Verify(repo => repo.DeleteAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(repo => repo.GetBySaleIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldHandleEmptySaleItems_WhenNoItemsExist()
        {
            // Arrange
            var saleId = _faker.Random.Int(1, 1000);

            _saleRepositoryMock
                .Setup(repo => repo.DeleteAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _saleItemRepositoryMock
                .Setup(repo => repo.GetBySaleIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SaleItem>());

            var command = new DeleteSaleCommand(saleId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _saleRepositoryMock.Verify(repo => repo.DeleteAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(repo => repo.GetBySaleIdAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
