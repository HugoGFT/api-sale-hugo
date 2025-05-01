using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Product title is required.")
                .MaximumLength(100)
                .WithMessage("Product title must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Product description is required.")
                .MaximumLength(500)
                .WithMessage("Product description must not exceed 500 characters.");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Product category is required.")
                .MaximumLength(50)
                .WithMessage("Product category must not exceed 50 characters.");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Product image URL is required.")
                .MaximumLength(200)
                .WithMessage("Product image URL must not exceed 200 characters.");

            RuleFor(x => x.Rate)
                .InclusiveBetween(0, 5)
                .WithMessage("Product rate must be between 0 and 5.");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product count must be 0 or greater.");
        }
    }
}
