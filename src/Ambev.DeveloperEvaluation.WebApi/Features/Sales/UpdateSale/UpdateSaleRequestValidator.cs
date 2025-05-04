using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("The sale date is required.")
                .Must(date => DateTime.TryParse(date, out _))
                .WithMessage("The sale date must be a valid date.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("The customer ID must be greater than 0.");

            RuleFor(x => x.Branch)
                .NotEmpty()
                .WithMessage("The branch is required.")
                .MaximumLength(100)
                .WithMessage("The branch must not exceed 100 characters.");

            RuleForEach(x => x.SaleItems)
                .SetValidator(new SaleItemDtoValidator());
        }
    }

    public class SaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
    {
        public SaleItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("The product ID must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("The quantity must be greater than 0.")
                .LessThan(21)
                .WithMessage("The quantity must be greater than 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The unit price must be greater than or equal to 0.");
        }
    }
}
