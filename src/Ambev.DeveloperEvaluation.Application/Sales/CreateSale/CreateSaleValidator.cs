using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Must(BeAValidDate).WithMessage("Date must be in a valid format.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId must be greater than 0.");

            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .MaximumLength(100).WithMessage("Branch must not exceed 100 characters.");

            RuleFor(x => x.SaleItems)
                .NotEmpty().WithMessage("At least one SaleItem is required.")
                .Must(items => items != null && items.Count > 0).WithMessage("SaleItems cannot be null or empty.");
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}
