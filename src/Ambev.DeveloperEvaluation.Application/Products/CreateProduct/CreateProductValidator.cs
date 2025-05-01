using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The Title field is required.")
                .MaximumLength(100).WithMessage("The Title field must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The Price must be greater than zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description field is required.")
                .MaximumLength(500).WithMessage("The Description field must not exceed 500 characters.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("The Category field is required.")
                .MaximumLength(50).WithMessage("The Category field must not exceed 50 characters.");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("The Image field is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("The Image field must be a valid URL.");

            RuleFor(x => x.Rate)
                .InclusiveBetween(0, 5).WithMessage("The Rate must be between 0 and 5.");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0).WithMessage("The Count must be zero or greater.");
        }
    }
}
