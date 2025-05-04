using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartCommandValidator()
        {
            RuleFor(command => command.UserID)
                .GreaterThan(0)
                .WithMessage("The user ID must be greater than zero.");

            RuleFor(command => command.Date)
                .NotEmpty()
                .WithMessage("The date is required.")
                .Must(BeAValidDate)
                .WithMessage("The provided date is not valid.");

            RuleFor(command => command.Products)
                .NotEmpty()
                .WithMessage("The product list cannot be empty.")
                .ForEach(product =>
                {
                    product.ChildRules(productRules =>
                    {
                        productRules.RuleFor(p => p.ProductId)
                            .GreaterThan(0)
                            .WithMessage("The product ID must be greater than zero.");

                        productRules.RuleFor(p => p.Quantity)
                            .GreaterThan(0)
                            .WithMessage("The product quantity must be greater than zero.")
                            .LessThan(21)
                            .WithMessage("The product quantity must be less than 20.");
                    });
                });
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}
