using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateProductHandler _handler;
        private readonly Faker<CreateProductCommand> _commandFaker;

        public CreateProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateProductHandler(_productRepositoryMock.Object, _mapperMock.Object);

            _commandFaker = new Faker<CreateProductCommand>()
                .RuleFor(c => c.Title, f => f.Commerce.ProductName())
                .RuleFor(c => c.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
                .RuleFor(c => c.Category, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Image, f => f.Image.PicsumUrl())
                .RuleFor(c => c.Rate, f => f.Random.Decimal(1, 5))
                .RuleFor(c => c.Count, f => f.Random.Int(1, 100));
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenCommandIsValid()
        {
            // Arrange
            var command = _commandFaker.Generate();
            var product = new Product();
            var createdProduct = new Product { Id = 1 };
            var result = new CreateProductResult { Id = 1 };

            _mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            _productRepositoryMock.Setup(r => r.CreateAsync(product, It.IsAny<CancellationToken>())).ReturnsAsync(createdProduct);
            _mapperMock.Setup(m => m.Map<CreateProductResult>(createdProduct)).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().BeEquivalentTo(result);
            _mapperMock.Verify(m => m.Map<Product>(command), Times.Once);
            _productRepositoryMock.Verify(r => r.CreateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<CreateProductResult>(createdProduct), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateProductCommand(); // Invalid command with missing required fields

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            _productRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
