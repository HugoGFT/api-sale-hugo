using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductValidator : AbstractValidator<GetProductCommand>
    {
        /// <summary>
        /// Initializes validation rules for GetUserCommand
        /// </summary>
        public GetProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}

