using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Faker _faker;

        public DeleteProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var productId = _faker.Random.Int(1, 1000);
            var command = new DeleteProductCommand(productId);

            _productRepositoryMock
                .Setup(repo => repo.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new DeleteProductHandler(_productRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new DeleteProductCommand(0); // Invalid ID
            var handler = new DeleteProductHandler(_productRepositoryMock.Object);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = _faker.Random.Int(1, 1000);
            var command = new DeleteProductCommand(productId);

            _productRepositoryMock
                .Setup(repo => repo.DeleteAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new DeleteProductHandler(_productRepositoryMock.Object);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {productId} not found");
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
