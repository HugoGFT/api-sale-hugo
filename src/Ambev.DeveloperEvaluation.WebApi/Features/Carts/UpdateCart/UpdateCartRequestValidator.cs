using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Validator for CreateCartRequest that defines validation rules for Cart creation.
/// </summary>
public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{

    public UpdateCartRequestValidator()
    {
        RuleFor(request => request.UserID)
            .GreaterThan(0)
            .WithMessage("UserID deve ser maior que 0.");

        RuleFor(request => request.Date)
            .NotEmpty()
            .WithMessage("Date é obrigatório.")
            .Must(date => DateTime.TryParse(date, out _))
            .WithMessage("Date deve ser uma data válida.");

        RuleForEach(request => request.Products)
            .ChildRules(product =>
            {
                product.RuleFor(p => p.ProductId)
                    .GreaterThan(0)
                    .WithMessage("ProductId deve ser maior que 0.");

                product.RuleFor(p => p.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity deve ser maior que 0.")
                    .LessThan(20)
                    .WithMessage("Quantity deve ser maior que 20.");
            });
    }
}