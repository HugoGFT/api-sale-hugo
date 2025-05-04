using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
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
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateProductHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenCommandIsValid()
        {
            // Arrange
            var faker = new Faker();
            var command = new UpdateProductCommand
            {
                Id = faker.Random.Int(1,1000),
                Title = faker.Commerce.ProductName(),
                Price = faker.Random.Decimal(1, 1000),
                Description = faker.Lorem.Paragraph(),
                Category = faker.Commerce.Categories(1).First(),
                Image = faker.Image.PicsumUrl(),
                Rate = faker.Random.Decimal(1, 5),
                Count = faker.Random.Int(1, 100)
            };

            var product = new Product
            {
                Id = command.Id,
                Title = command.Title,
                Price = command.Price,
                Description = command.Description,
                Category = command.Category,
                Image = command.Image,
                Rate = command.Rate,
                Count = command.Count
            };

            var updatedProduct = new Product
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

            var result = new UpdateProductResult
            {
                Id = updatedProduct.Id,
                Title = updatedProduct.Title,
                Price = updatedProduct.Price,
                Description = updatedProduct.Description,
                Category = updatedProduct.Category,
                Image = updatedProduct.Image,
                Rate = updatedProduct.Rate,
                Count = updatedProduct.Count
            };

            _mapperMock.Setup(m => m.Map<Product>(command)).Returns(product);
            _productRepositoryMock.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).ReturnsAsync(updatedProduct);
            _mapperMock.Setup(m => m.Map<UpdateProductResult>(updatedProduct)).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(command.Id, response.Id);
            Assert.Equal(command.Title, response.Title);
            Assert.Equal(command.Price, response.Price);
            Assert.Equal(command.Description, response.Description);
            Assert.Equal(command.Category, response.Category);
            Assert.Equal(command.Image, response.Image);
            Assert.Equal(command.Rate, response.Rate);
            Assert.Equal(command.Count, response.Count);

            _mapperMock.Verify(m => m.Map<Product>(command), Times.Once);
            _productRepositoryMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<UpdateProductResult>(updatedProduct), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateProductCommand(); // Invalid command with default values
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Title", "Title is required.")
            });

            var validatorMock = new Mock<IValidator<UpdateProductCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
