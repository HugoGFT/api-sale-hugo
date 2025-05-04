using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class GetSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleItemRepository> _saleItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetSaleHandler(_saleRepositoryMock.Object, _mapperMock.Object, _saleItemRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSaleResult_WhenSaleExists()
        {
            // Arrange
            var faker = new Faker();
            var saleId = faker.Random.Int(1, 1000);

            var sale = new Sale
            {
                Id = saleId,
                Date = faker.Date.Recent().ToString(),
                CustomerId = faker.Random.Int(1, 100),
                Branch = faker.Company.CompanyName(),
                TotalAmount = faker.Finance.Amount(),
                TotalDiscount = faker.Finance.Amount(),
                TotalWithDiscount = faker.Finance.Amount(),
                IsCancelled = faker.Random.Bool()
            };

            var saleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    Id = faker.Random.Int(1, 1000),
                    SaleId = saleId,
                    ProductId = faker.Random.Int(1, 100),
                    Quantity = faker.Random.Int(1, 10),
                    UnitPrice = faker.Finance.Amount(),
                    Discount = faker.Finance.Amount(),
                    IsCancelled = faker.Random.Bool(),
                    TotalAmount = faker.Finance.Amount(),
                    Total = faker.Finance.Amount()
                }
            };

            var saleResult = new GetSaleResult
            {
                Id = sale.Id,
                Date = sale.Date,
                CustomerId = sale.CustomerId,
                Branch = sale.Branch,
                TotalAmount = sale.TotalAmount,
                TotalDiscount = sale.TotalDiscount,
                TotalWithDiscount = sale.TotalWithDiscount,
                IsCancelled = sale.IsCancelled,
                SaleItems = new List<SaleItemDto>
                {
                    new SaleItemDto
                    {
                        ProductId = saleItems[0].ProductId,
                        Quantity = saleItems[0].Quantity,
                        UnitPrice = saleItems[0].UnitPrice,
                        Discount = saleItems[0].Discount,
                        IsCancelled = saleItems[0].IsCancelled,
                        Total = saleItems[0].Total
                    }
                }
            };

            _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _saleItemRepositoryMock.Setup(repo => repo.GetBySaleIdAsync(saleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(saleItems);

            _mapperMock.Setup(mapper => mapper.Map<GetSaleResult>(sale))
                .Returns(saleResult);

            _mapperMock.Setup(mapper => mapper.Map<List<SaleItemDto>>(saleItems))
                .Returns(saleResult.SaleItems);

            var command = new GetSaleCommand { Id = saleId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
            result.SaleItems.Should().HaveCount(1);
            _saleRepositoryMock.Verify(repo => repo.GetByIdAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
            _saleItemRepositoryMock.Verify(repo => repo.GetBySaleIdAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenSaleDoesNotExist()
        {
            // Arrange
            var faker = new Faker();
            var saleId = faker.Random.Int(1, 1000);

            _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId, It.IsAny<CancellationToken>()));

            var command = new GetSaleCommand { Id = saleId };

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {saleId} not found");
            _saleRepositoryMock.Verify(repo => repo.GetByIdAsync(saleId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new GetSaleCommand { Id = 0 }; // Invalid ID
            var validator = new GetSaleValidator();

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
