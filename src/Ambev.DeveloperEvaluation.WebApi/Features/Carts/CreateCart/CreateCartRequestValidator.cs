using Ambev.DeveloperEvaluation.Domain.Validation;
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
            .WithMessage("UserID deve ser maior que 0.");

        RuleFor(request => request.Date)
            .NotEmpty()
            .WithMessage("A data é obrigatória.")
            .Must(date => DateTime.TryParse(date, out _))
            .WithMessage("A data deve estar em um formato válido.");

        RuleFor(request => request.Products)
            .NotEmpty()
            .WithMessage("A lista de produtos não pode estar vazia.")
            .ForEach(productRule =>
            {
                productRule.ChildRules(product =>
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
            });
    }
}