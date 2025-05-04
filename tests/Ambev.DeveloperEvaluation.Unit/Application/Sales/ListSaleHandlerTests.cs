using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using MediatR;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class ListSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleItemRepository> _saleItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ListSaleHandler _handler;

        public ListSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new ListSaleHandler(
                _saleRepositoryMock.Object,
                _mapperMock.Object,
                _saleItemRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListSaleResult_WhenSalesExist()
        {
            // Arrange
            var command = new Faker<ListSaleCommand>()
                .RuleFor(c => c.Page, f => f.Random.Int(1, 10))
                .RuleFor(c => c.PageSize, f => f.Random.Int(1, 50))
                .RuleFor(c => c.Order, f => f.Random.Word())
                .Generate();

            var filter = new Faker<ListSaleFilter>()
                .RuleFor(f => f.Page, command.Page)
                .RuleFor(f => f.PageSize, command.PageSize)
                .RuleFor(f => f.Order, command.Order)
                .Generate();

            var saleResultDto = new Faker<ListSaleResultDto>()
                .RuleFor(r => r.TotalItems, f => f.Random.Int(1, 100))
                .RuleFor(r => r.TotalPages, f => f.Random.Int(1, 10))
                .RuleFor(r => r.CurrentPage, f => f.Random.Int(1, 10))
                .RuleFor(r => r.Data, f => new List<Sale>
                {
                    new Faker<Sale>()
                        .RuleFor(s => s.Date, f => f.Date.Past().ToString("yyyy-MM-dd"))
                        .RuleFor(s => s.CustomerId, f => f.Random.Int(1, 100))
                        .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                        .RuleFor(s => s.TotalAmount, f => f.Finance.Amount())
                        .RuleFor(s => s.TotalDiscount, f => f.Finance.Amount())
                        .RuleFor(s => s.TotalWithDiscount, f => f.Finance.Amount())
                        .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
                        .Generate()
                })
                .Generate();

            var saleItems = new Faker<SaleItem>()
                .RuleFor(i => i.Id, f => f.Random.Int(1, 100))
                .RuleFor(i => i.SaleId, f => f.Random.Int(1, 100))
                .RuleFor(i => i.ProductId, f => f.Random.Int(1, 100))
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(i => i.UnitPrice, f => f.Finance.Amount())
                .RuleFor(i => i.Discount, f => f.Finance.Amount())
                .RuleFor(i => i.TotalAmount, f => f.Finance.Amount())
                .Generate(5);

            _mapperMock.Setup(m => m.Map<ListSaleFilter>(command)).Returns(filter);
            _saleRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleResultDto);
            _mapperMock.Setup(m => m.Map<ListSaleResult>(saleResultDto))
                .Returns(new ListSaleResult
                {
                    TotalItems = saleResultDto.TotalItems,
                    TotalPages = saleResultDto.TotalPages,
                    CurrentPage = saleResultDto.CurrentPage,
                    Data = saleResultDto.Data.Select(s => new GetSaleResult
                    {
                        Id = s.Id,
                        Date = s.Date,
                        CustomerId = s.CustomerId,
                        Branch = s.Branch,
                        TotalAmount = s.TotalAmount,
                        TotalDiscount = s.TotalDiscount,
                        TotalWithDiscount = s.TotalWithDiscount,
                        IsCancelled = s.IsCancelled,
                        SaleItems = new List<SaleItemDto>()
                    })
                });

            _saleItemRepositoryMock.Setup(r => r.GetBySaleIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleItems);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(saleResultDto.TotalItems, result.TotalItems);
            Assert.Equal(saleResultDto.TotalPages, result.TotalPages);
            Assert.Equal(saleResultDto.CurrentPage, result.CurrentPage);
            Assert.NotEmpty(result.Data);
            _saleRepositoryMock.Verify(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(r => r.GetBySaleIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSalesNotFound()
        {
            // Arrange
            var command = new Faker<ListSaleCommand>()
                .RuleFor(c => c.Page, f => f.Random.Int(1, 10))
                .RuleFor(c => c.PageSize, f => f.Random.Int(1, 50))
                .RuleFor(c => c.Order, f => f.Random.Word())
                .Generate();

            var filter = new Faker<ListSaleFilter>()
                .RuleFor(f => f.Page, command.Page)
                .RuleFor(f => f.PageSize, command.PageSize)
                .RuleFor(f => f.Order, command.Order)
                .Generate();

            _mapperMock.Setup(m => m.Map<ListSaleFilter>(command)).Returns(filter);
            _saleRepositoryMock.Setup(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            _saleRepositoryMock.Verify(r => r.GetByFilterAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
