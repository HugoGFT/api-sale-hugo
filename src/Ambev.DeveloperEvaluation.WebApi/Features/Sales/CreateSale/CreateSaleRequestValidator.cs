using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("The date is required.")
                .Must(BeAValidDate).WithMessage("The date must be in a valid format.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("The customer ID must be greater than zero.");

            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("The branch is required.");

            RuleForEach(x => x.SaleItems).SetValidator(new SaleItemDtoValidator());
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }

    public class SaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
    {
        public SaleItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("The product ID must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The quantity must be greater than zero.")
                .LessThan(21).WithMessage("The quantity must be greater than 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("The unit price must be greater than zero.");
        }
    }
}
