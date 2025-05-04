using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
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

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class GetProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetProductHandler _handler;

        public GetProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var faker = new Faker();
            var productId = faker.Random.Int(1, 1000);

            var product = new Faker<Product>()
                .RuleFor(p => p.Id, productId)
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1).First())
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Rate, f => f.Random.Decimal(1, 5))
                .RuleFor(p => p.Count, f => f.Random.Int(1, 100))
                .Generate();

            var getProductResult = new GetProductResult
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                Image = product.Image,
                Rate = product.Rate,
                Count = product.Count
            };

            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(mapper => mapper.Map<GetProductResult>(product))
                .Returns(getProductResult);

            var command = new GetProductCommand(productId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(product.Title, result.Title);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Category, result.Category);
            Assert.Equal(product.Image, result.Image);
            Assert.Equal(product.Rate, result.Rate);
            Assert.Equal(product.Count, result.Count);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var faker = new Faker();
            var productId = faker.Random.Int(1, 1000);

            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var command = new GetProductCommand(productId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new GetProductCommand(0); // Invalid ID
            var validator = new GetProductValidator();
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Id", "Id must be greater than 0") });

            var validatorMock = new Mock<IValidator<GetProductCommand>>();
            validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
