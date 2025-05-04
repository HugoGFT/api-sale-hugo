using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class GetCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductCartRepository> _productCartRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetCartHandler _handler;

        public GetCartHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productCartRepositoryMock = new Mock<IProductCartRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetCartHandler(_cartRepositoryMock.Object, _mapperMock.Object, _productCartRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var faker = new Faker();
            var cartId = faker.Random.Int(1, 1000);

            var cart = new Cart
            {
                Id = cartId,
                UserID = faker.Random.Int(1, 100),
                Date = faker.Date.Recent().ToString("yyyy-MM-dd")
            };

            var productCarts = new List<ProductCart>
            {
                new ProductCart { IdCart = cartId, IdProduct = faker.Random.Int(1, 100), Count = faker.Random.Int(1, 10) }
            };

            var expectedResult = new GetCartResult
            {
                Id = cartId,
                UserID = cart.UserID,
                Date = cart.Date,
                Products = productCarts.Select(pc => new ProductCartDto
                {
                    ProductId = pc.IdProduct,
                    Quantity = pc.Count
                }).ToList()
            };

            _cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _productCartRepositoryMock.Setup(repo => repo.GetByCartIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCarts!);

            _mapperMock.Setup(mapper => mapper.Map<GetCartResult>(cart))
                .Returns(expectedResult);

            _mapperMock.Setup(mapper => mapper.Map<List<ProductCartDto>>(productCarts))
                .Returns(expectedResult.Products);

            var command = new GetCartCommand { Id = cartId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.UserID, result.UserID);
            Assert.Equal(expectedResult.Date, result.Date);
            Assert.Equal(expectedResult.Products.Count, result.Products.Count);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new GetCartCommand { Id = 0 }; // Invalid ID
            var validator = new GetCartValidator();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(GetCartCommand.Id), "Id must be greater than 0")
            });

            var validatorMock = new Mock<IValidator<GetCartCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenCartDoesNotExist()
        {
            // Arrange
            var faker = new Faker();
            var cartId = faker.Random.Int(1, 1000);

            _cartRepositoryMock.Setup(repo => repo.GetByIdAsync(cartId, It.IsAny<CancellationToken>()));

            var command = new GetCartCommand { Id = cartId };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
