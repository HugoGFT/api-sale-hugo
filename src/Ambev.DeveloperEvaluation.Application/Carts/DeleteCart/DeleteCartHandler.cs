using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResponse>
    {
        private readonly ICartRepository _CartRepository;

        /// <summary>
        /// Initializes a new instance of DeleteCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="validator">The validator for DeleteCartCommand</param>
        public DeleteCartHandler(
            ICartRepository CartRepository)
        {
            _CartRepository = CartRepository;
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

            var success = await _CartRepository.DeleteAsync(request.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

            return new DeleteCartResponse { Success = true };
        }
    }
}
