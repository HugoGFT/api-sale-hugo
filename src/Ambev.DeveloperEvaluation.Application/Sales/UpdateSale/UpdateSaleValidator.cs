using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("The Id must be greater than 0.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("The date is required.")
                .Must(BeAValidDate).WithMessage("The date must be in a valid format.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("The customer Id must be greater than 0.");

            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("The branch is required.")
                .MaximumLength(100).WithMessage("The branch must not exceed 100 characters.");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("The total amount must be greater than or equal to 0.");

            RuleFor(x => x.TotalDiscount)
                .GreaterThanOrEqualTo(0).WithMessage("The total discount must be greater than or equal to 0.");

            RuleFor(x => x.TotalWithDiscount)
                .GreaterThanOrEqualTo(0).WithMessage("The total with discount must be greater than or equal to 0.")
                .Equal(x => x.TotalAmount - x.TotalDiscount).WithMessage("The total with discount must equal the total amount minus the total discount.");

            RuleForEach(x => x.SaleItems)
                .SetValidator(new SaleItemValidator());
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }

    public class SaleItemValidator : AbstractValidator<SaleItemDto>
    {
        public SaleItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("The product Id must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("The quantity must be greater than 0.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("The unit price must be greater than or equal to 0.");
        }
    }
}
