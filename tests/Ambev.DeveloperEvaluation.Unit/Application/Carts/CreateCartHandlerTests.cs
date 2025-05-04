using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
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
    public class CreateCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductCartRepository> _productCartRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateCartHandler _handler;

        public CreateCartHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productCartRepositoryMock = new Mock<IProductCartRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateCartHandler(
                _cartRepositoryMock.Object,
                _mapperMock.Object,
                _productCartRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateCartAndReturnResult_WhenCommandIsValid()
        {
            // Arrange
            var faker = new Faker();
            var command = new CreateCartCommand
            {
                UserID = faker.Random.Int(1, 1000),
                Date = faker.Date.Recent().ToString("yyyy-MM-dd"),
                Products = new List<ProductCartDto>
                {
                    new ProductCartDto { ProductId = faker.Random.Int(1, 100), Quantity = faker.Random.Int(1, 10) }
                }
            };

            var cart = new Cart { Id = faker.Random.Int(1, 1000), UserID = command.UserID, Date = command.Date };
            var createdCart = new Cart { Id = cart.Id, UserID = cart.UserID, Date = cart.Date };

            var productCarts = command.Products.Select(p => new ProductCart
            {
                IdUser = command.UserID,
                IdCart = cart.Id,
                IdProduct = p.ProductId,
                Count = p.Quantity
            }).ToList();

            var productCartDtos = productCarts.Select(pc => new ProductCartDto
            {
                ProductId = pc.IdProduct,
                Quantity = pc.Count
            }).ToList();

            _mapperMock.Setup(m => m.Map<Cart>(command)).Returns(cart);
            _cartRepositoryMock.Setup(r => r.CreateAsync(cart, It.IsAny<CancellationToken>())).ReturnsAsync(createdCart);
            _mapperMock.Setup(m => m.Map<List<ProductCart>>(command.Products)).Returns(productCarts);
            _productCartRepositoryMock.Setup(r => r.GetByCartIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(productCarts.Cast<ProductCart?>().ToList());
            _mapperMock.Setup(m => m.Map<CreateCartResult>(createdCart)).Returns(new CreateCartResult { Id = createdCart.Id, UserID = createdCart.UserID, Date = createdCart.Date });
            _mapperMock.Setup(m => m.Map<List<ProductCartDto>>(productCarts)).Returns(productCartDtos);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdCart.Id, result.Id);
            Assert.Equal(createdCart.UserID, result.UserID);
            Assert.Equal(createdCart.Date, result.Date);
            Assert.Equal(productCartDtos.Count, result.Products.Count);

            _cartRepositoryMock.Verify(r => r.CreateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
            _productCartRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<ProductCart>(), It.IsAny<CancellationToken>()), Times.Exactly(productCarts.Count));
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateCartCommand();
            var validator = new CreateCartCommandValidator();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("UserID", "UserID is required")
            });

            _mapperMock.Setup(m => m.Map<Cart>(command)).Throws(new ValidationException(validationResult.Errors));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
