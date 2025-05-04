using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
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
    public class CreateSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<ISaleItemRepository> _saleItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _saleItemRepositoryMock = new Mock<ISaleItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateSaleHandler(
                _saleRepositoryMock.Object,
                _mapperMock.Object,
                _saleItemRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateSaleSuccessfully()
        {
            // Arrange
            var faker = new Faker();
            var command = new Faker<CreateSaleCommand>()
                .RuleFor(c => c.Date, f => f.Date.Recent().ToString("yyyy-MM-dd"))
                .RuleFor(c => c.CustomerId, f => f.Random.Int(1, 1000))
                .RuleFor(c => c.Branch, f => f.Company.CompanyName())
                .RuleFor(c => c.IsCancelled, f => false)
                .RuleFor(c => c.SaleItems, f => new Faker<SaleItemDto>()
                    .RuleFor(i => i.ProductId, f => f.Random.Int(1, 100))
                    .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                    .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100))
                    .RuleFor(i => i.Discount, f => f.Finance.Amount(0, 10))
                    .RuleFor(i => i.IsCancelled, f => false)
                    .RuleFor(i => i.Total, (f, i) => i.Quantity * i.UnitPrice - i.Discount)
                    .Generate(3))
                .Generate();

            var sale = new Sale
            {
                Id = faker.Random.Int(1, 1000),
                Date = command.Date ?? string.Empty, // Resolved nullable reference issue
                CustomerId = command.CustomerId,
                Branch = command.Branch ?? string.Empty, // Resolved nullable reference issue
                TotalAmount = command.SaleItems.Sum(i => i.Total),
                TotalDiscount = command.SaleItems.Sum(i => i.Discount),
                TotalWithDiscount = command.SaleItems.Sum(i => i.Total) - command.SaleItems.Sum(i => i.Discount),
                IsCancelled = command.IsCancelled
            };

            _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(sale);
            _saleRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            var saleItems = command.SaleItems.Select(i => new SaleItem
            {
                SaleId = sale.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Total = i.Total
            }).ToList();

            _mapperMock.Setup(m => m.Map<List<SaleItem>>(command.SaleItems)).Returns(saleItems);
            _saleItemRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<SaleItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SaleItem item, CancellationToken _) => item);

            _mapperMock.Setup(m => m.Map<CreateSaleResult>(sale)).Returns(new CreateSaleResult
            {
                Id = sale.Id,
                Date = sale.Date ?? string.Empty, // Resolved nullable reference issue
                CustomerId = sale.CustomerId,
                Branch = sale.Branch ?? string.Empty, // Resolved nullable reference issue
                TotalAmount = sale.TotalAmount,
                TotalDiscount = sale.TotalDiscount,
                TotalWithDiscount = sale.TotalWithDiscount,
                IsCancelled = sale.IsCancelled,
                SaleItems = command.SaleItems
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sale.Id, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateSaleCommand();
            var validator = new CreateSaleCommandValidator();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("CustomerId", "CustomerId is required")
            });

            var validatorMock = new Mock<IValidator<CreateSaleCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
