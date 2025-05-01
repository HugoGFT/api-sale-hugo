using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
    {
        /// <summary>
        /// Initializes validation rules for UpdateCartCommand
        /// </summary>
        public UpdateCartCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Cart ID must be greater than 0.");

            RuleFor(x => x.UserID)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.")
                .Must(BeAValidDate)
                .WithMessage("Date must be in a valid format.");

            RuleFor(x => x.Products)
                .NotEmpty()
                .WithMessage("At least one product must be included in the cart.")
                .ForEach(product =>
                {
                    product.ChildRules(productRules =>
                    {
                        productRules.RuleFor(p => p.ProductId)
                            .GreaterThan(0)
                            .WithMessage("Product ID must be greater than 0.");

                        productRules.RuleFor(p => p.Quantity)
                            .GreaterThan(0)
                            .WithMessage("Product quantity must be greater than 0.")
                            .LessThan(20)
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
