using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class UpdateCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductCartRepository> _productCartRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateCartHandler _handler;

        public UpdateCartHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productCartRepositoryMock = new Mock<IProductCartRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateCartHandler(
                _cartRepositoryMock.Object,
                _mapperMock.Object,
                _productCartRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateCartAndReturnResult_WhenCommandIsValid()
        {
            // Arrange
            var faker = new Faker();
            var command = new UpdateCartCommand
            {
                Id = faker.Random.Int(1, 1000),
                UserID = faker.Random.Int(1, 1000),
                Date = faker.Date.Recent().ToString(),
                Products = new List<ProductCartDto>
                {
                    new ProductCartDto { ProductId = faker.Random.Int(1, 1000), Quantity = faker.Random.Int(1, 10) }
                }
            };

            var existingCart = new Cart { Id = command.Id, UserID = command.UserID, Date = command.Date };
            var updatedCart = new Cart { Id = command.Id, UserID = command.UserID, Date = command.Date };
            var productCarts = new List<ProductCart>
            {
                new ProductCart { IdProduct = command.Products.First().ProductId, Count = command.Products.First().Quantity }
            };

            _cartRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCart);

            _cartRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedCart);

            _productCartRepositoryMock.Setup(repo => repo.GetByCartIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductCart?>());

            _mapperMock.Setup(mapper => mapper.Map<Cart>(command)).Returns(updatedCart);
            _mapperMock.Setup(mapper => mapper.Map<List<ProductCart>>(command.Products)).Returns(productCarts);
            _mapperMock.Setup(mapper => mapper.Map<UpdateCartResult>(updatedCart))
                .Returns(new UpdateCartResult { Id = updatedCart.Id, UserID = updatedCart.UserID, Date = updatedCart.Date });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedCart.Id, result.Id);
            Assert.Equal(updatedCart.UserID, result.UserID);
            _cartRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateCartCommand();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Id", "Id is required")
            });

            var validatorMock = new Mock<IValidator<UpdateCartCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
