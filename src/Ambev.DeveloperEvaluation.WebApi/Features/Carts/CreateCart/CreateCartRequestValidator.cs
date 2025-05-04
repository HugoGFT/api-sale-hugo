using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Validator for CreateCartRequest that defines validation rules for Cart creation.
/// </summary>
public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{

    public CreateCartRequestValidator()
    {
        RuleFor(request => request.UserID)
            .GreaterThan(0)
            .WithMessage("UserID must be greater than 0.");

        RuleFor(request => request.Date)
            .NotEmpty()
            .WithMessage("Date is required.")
            .Must(date => DateTime.TryParse(date, out _))
            .WithMessage("Date must be in a valid format.");

        RuleFor(request => request.Products)
            .NotEmpty()
            .WithMessage("The product list cannot be empty.")
            .ForEach(productRule =>
            {
                productRule.ChildRules(product =>
                {
                    product.RuleFor(p => p.ProductId)
                        .GreaterThan(0)
                        .WithMessage("ProductId must be greater than 0.");

                    product.RuleFor(p => p.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than 0.")
                        .LessThan(21)
                        .WithMessage("Quantity must be less than 21.");
                });
            });
    }
}