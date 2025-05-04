using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
    {
        /// <summary>
        /// Initializes validation rules for DeleteUserRequest
        /// </summary>
        public DeleteSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}
