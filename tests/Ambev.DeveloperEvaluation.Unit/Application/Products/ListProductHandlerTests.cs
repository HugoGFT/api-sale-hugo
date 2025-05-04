using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProduct;
using Ambev.DeveloperEvaluation.Domain.Dto.ProductDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class ListProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ListProductHandler _handler;

        public ListProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new ListProductHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedResult_WhenProductsAreFound()
        {
            // Arrange
            var command = new Faker<ListProductCommand>()
                .RuleFor(c => c.Page, f => f.Random.Int(1, 10))
                .RuleFor(c => c.PageSize, f => f.Random.Int(1, 50))
                .RuleFor(c => c.Order, f => f.Random.Word())
                .Generate();

            var filter = new ListProductFilter
            {
                Page = command.Page,
                PageSize = command.PageSize,
                Order = command.Order
            };

            var productResultDto = new ListProductResultDto
            (
                50,
                5,
                1,
                new Faker<Product>()
                    .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
                    .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                    .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                    .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                    .RuleFor(p => p.Rate, f => f.Random.Decimal(1, 5))
                    .RuleFor(p => p.Count, f => f.Random.Int(1, 100))
                    .Generate(5)
            );

            var expectedResult = new ListProductResult
            {
                TotalItems = productResultDto.TotalItems,
                TotalPages = productResultDto.TotalPages,
                CurrentPage = productResultDto.CurrentPage,
                Data = new Faker<GetProductResult>()
                    .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
                    .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                    .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                    .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                    .RuleFor(p => p.Rate, f => f.Random.Decimal(1, 5))
                    .RuleFor(p => p.Count, f => f.Random.Int(1, 100))
                    .Generate(5)
            };

            _mapperMock.Setup(m => m.Map<ListProductFilter>(command)).Returns(filter);
            _productRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(productResultDto);
            _mapperMock.Setup(m => m.Map<ListProductResult>(productResultDto)).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mapperMock.Verify(m => m.Map<ListProductFilter>(command), Times.Once);
            _productRepositoryMock.Verify(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ListProductResult>(productResultDto), Times.Once);
        }
    }
}
