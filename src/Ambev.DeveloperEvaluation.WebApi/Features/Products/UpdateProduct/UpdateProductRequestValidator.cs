using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductRequest that defines validation rules for Product creation.
/// </summary>
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Title: Required, length between 3 and 100 characters
    /// - Price: Must be greater than 0
    /// - Description: Optional, but if provided, must be at least 10 characters
    /// - Category: Required
    /// - Image: Optional, but if provided, must be a valid URL
    /// - Rate: Must be between 0 and 5
    /// - Count: Must be a non-negative integer
    /// </remarks>
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.Description)
            .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description must be at least 10 characters if provided.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5.");

        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Count must be a non-negative integer.");
    }
}