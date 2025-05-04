using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
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

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class UpdateSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleItemRepository> _saleItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateSaleHandler(
                _saleRepositoryMock.Object,
                _mapperMock.Object,
                _saleItemRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateSaleSuccessfully()
        {
            // Arrange
            var faker = new Faker();
            var command = new Faker<UpdateSaleCommand>()
                .RuleFor(c => c.Id, faker.Random.Int(1, 1000))
                .RuleFor(c => c.Date, faker.Date.Recent().ToString("yyyy-MM-dd"))
                .RuleFor(c => c.CustomerId, faker.Random.Int(1, 100))
                .RuleFor(c => c.Branch, faker.Address.City())
                .RuleFor(c => c.SaleItems, f => new Faker<SaleItemDto>()
                    .RuleFor(i => i.ProductId, f.Random.Int(1, 100))
                    .RuleFor(i => i.Quantity, f.Random.Int(1, 10))
                    .RuleFor(i => i.UnitPrice, f.Random.Decimal(1, 100))
                    .RuleFor(i => i.Discount, f.Random.Decimal(0, 10))
                    .RuleFor(i => i.IsCancelled, f.Random.Bool())
                    .Generate(3))
                .Generate();

            var existingSale = new Sale { Id = command.Id };
            var mappedSale = new Sale { Id = command.Id };
            var updatedSale = new Sale { Id = command.Id };
            var saleItems = command.SaleItems.Select(i => new SaleItem { ProductId = i.ProductId }).ToList();

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingSale);
            _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(mappedSale);
            _saleRepositoryMock.Setup(r => r.UpdateAsync(mappedSale, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedSale);
            _mapperMock.Setup(m => m.Map<List<SaleItem>>(command.SaleItems)).Returns(saleItems);
            _saleItemRepositoryMock.Setup(r => r.GetBySaleIdAsync(updatedSale.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SaleItem?>());
            _mapperMock.Setup(m => m.Map<UpdateSaleResult>(updatedSale))
                .Returns(new UpdateSaleResult { Id = updatedSale.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedSale.Id, result.Id);
            _saleRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            _saleRepositoryMock.Verify(r => r.UpdateAsync(mappedSale, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new UpdateSaleCommand();
            var validator = new UpdateSaleCommandValidator();
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Id", "Id is required") });

            _mapperMock.Setup(m => m.Map<Sale>(command)).Throws(new ValidationException(validationResult.Errors));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
