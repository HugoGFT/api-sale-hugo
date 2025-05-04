using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCart;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class ListCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IProductCartRepository> _productCartRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ListCartHandler _handler;

        public ListCartHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _productCartRepositoryMock = new Mock<IProductCartRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new ListCartHandler(
                _cartRepositoryMock.Object,
                _mapperMock.Object,
                _productCartRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListCartResult_WhenCartExists()
        {
            // Arrange
            var command = new Faker<ListCartCommand>()
                .RuleFor(c => c.Page, f => f.Random.Int(1, 10))
                .RuleFor(c => c.PageSize, f => f.Random.Int(1, 50))
                .RuleFor(c => c.Order, f => f.Random.Word())
                .Generate();

            var filter = new Faker<ListCartFilter>()
                .RuleFor(f => f.Page, command.Page)
                .RuleFor(f => f.PageSize, command.PageSize)
                .RuleFor(f => f.Order, command.Order)
                .Generate();

            var cartResultDto = new Faker<ListCartResultDto>()
                .RuleFor(c => c.TotalItems, f => f.Random.Int(1, 100))
                .RuleFor(c => c.TotalPages, f => f.Random.Int(1, 10))
                .RuleFor(c => c.CurrentPage, f => f.Random.Int(1, 10))
                .RuleFor(c => c.Data, f => new List<Cart>
                {
                    new Faker<Cart>()
                        .RuleFor(c => c.Id, f => f.Random.Int(1, 100))
                        .RuleFor(c => c.UserID, f => f.Random.Int(1, 1000))
                        .RuleFor(c => c.Date, f => f.Date.Past().ToString("yyyy-MM-dd"))
                        .Generate()
                })
                .Generate();

            var productCartDtos = new Faker<ProductCart>()
                .RuleFor(p => p.IdProduct, f => f.Random.Int(1, 100))
                .RuleFor(p => p.Count, f => f.Random.Int(1, 10))
                .RuleFor(p => p.IdCart, f => f.Random.Int(1, 10))
                .RuleFor(p => p.IdUser, f => f.Random.Int(1, 10))
                .Generate(3);

            _mapperMock.Setup(m => m.Map<ListCartFilter>(command)).Returns(filter);
            _cartRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartResultDto);
            _mapperMock.Setup(m => m.Map<ListCartResult>(cartResultDto))
                .Returns(new ListCartResult
                {
                    TotalItems = cartResultDto.TotalItems,
                    TotalPages = cartResultDto.TotalPages,
                    CurrentPage = cartResultDto.CurrentPage,
                    Data = new List<GetCartResult>
                    {
                        new GetCartResult
                        {
                            Id = cartResultDto.Data.First().Id,
                            UserID = cartResultDto.Data.First().UserID,
                            Date = cartResultDto.Data.First().Date,
                            Products = new List<ProductCartDto>()
                        }
                    }
                });
            _productCartRepositoryMock.Setup(r => r.GetByCartIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCartDtos);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCartNotFound()
        {
            // Arrange
            var command = new Faker<ListCartCommand>()
                .RuleFor(c => c.Page, f => f.Random.Int(1, 10))
                .RuleFor(c => c.PageSize, f => f.Random.Int(1, 50))
                .RuleFor(c => c.Order, f => f.Random.Word())
                .Generate(1).FirstOrDefault();

            var filter = new Faker<ListCartFilter>()
                .RuleFor(f => f.Page, command.Page)
                .RuleFor(f => f.PageSize, command.PageSize)
                .RuleFor(f => f.Order, command.Order)
                .Generate(1).FirstOrDefault();

            _mapperMock.Setup(m => m.Map<ListCartFilter>(command)).Returns(filter);
            _cartRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ListCartResultDto)null); 

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Cart not found");
        }
    }
}
