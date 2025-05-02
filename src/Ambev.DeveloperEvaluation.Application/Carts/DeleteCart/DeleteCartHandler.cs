using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductCartRepository _productCartRepository; 

        /// <summary>
        /// Initializes a new instance of DeleteCartHandler
        /// </summary>
        /// <param name="cartRepository">The Cart repository</param>
        /// <param name="validator">The validator for DeleteCartCommand</param>
        public DeleteCartHandler(
            ICartRepository cartRepository, IProductCartRepository productCartRepository)
        {
            _cartRepository = cartRepository;
            _productCartRepository = productCartRepository;
        }

        /// <summary>
        /// Handles the DeleteCartCommand request
        /// </summary>
        /// <param name="request">The DeleteCart command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the delete operation</returns>
        public async Task<DeleteCartResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteCartValidator();
            var validationResult = await validator.ValidateAsync(request, options => options.ThrowOnFailures(), cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var success = await _cartRepository.DeleteAsync(request.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Cart with ID {request.Id} not found");
            var products = await _productCartRepository.GetByCartIdAsync(request.Id, cancellationToken);
            if (products != null && products.Any())
            {
                foreach (var product in products)
                {
                    await _productCartRepository.DeleteAsync(product.Id, cancellationToken);
                }
            }

            return new DeleteCartResponse { Success = true };
        }
    }
}
