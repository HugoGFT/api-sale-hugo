using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class DeleteCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductCartRepository> _productCartRepositoryMock;
        private readonly Faker _faker;

        public DeleteCartHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productCartRepositoryMock = new Mock<IProductCartRepository>();
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_ShouldDeleteCartAndProducts_WhenCartExists()
        {
            // Arrange
            var cartId = _faker.Random.Int(1, 1000);
            var products = new List<ProductCart>
            {
                new ProductCart { Id = _faker.Random.Int(1, 1000), IdCart = cartId },
                new ProductCart { Id = _faker.Random.Int(1, 1000), IdCart = cartId }
            };

            _cartRepositoryMock
                .Setup(repo => repo.DeleteAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productCartRepositoryMock
                .Setup(repo => repo.GetByCartIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products!); 

            _productCartRepositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new DeleteCartHandler(_cartRepositoryMock.Object, _productCartRepositoryMock.Object);
            var command = new DeleteCartCommand(cartId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            _cartRepositoryMock.Verify(repo => repo.DeleteAsync(cartId, It.IsAny<CancellationToken>()), Times.Once);
            _productCartRepositoryMock.Verify(repo => repo.GetByCartIdAsync(cartId, It.IsAny<CancellationToken>()), Times.Once);
            _productCartRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(products.Count));
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenCartDoesNotExist()
        {
            // Arrange
            var cartId = _faker.Random.Int(1, 1000);

            _cartRepositoryMock
                .Setup(repo => repo.DeleteAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new DeleteCartHandler(_cartRepositoryMock.Object, _productCartRepositoryMock.Object);
            var command = new DeleteCartCommand(cartId);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Cart with ID {cartId} not found");
            _cartRepositoryMock.Verify(repo => repo.DeleteAsync(cartId, It.IsAny<CancellationToken>()), Times.Once);
            _productCartRepositoryMock.Verify(repo => repo.GetByCartIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new DeleteCartCommand(0); // Invalid ID
            var handler = new DeleteCartHandler(_cartRepositoryMock.Object, _productCartRepositoryMock.Object);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
            _cartRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _productCartRepositoryMock.Verify(repo => repo.GetByCartIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
