using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleItemRepository _saleItemRepository;

        /// <summary>
        /// Initializes a new instance of DeleteSaleHandler
        /// </summary>
        /// <param name="SaleRepository">The Sale repository</param>
        /// <param name="validator">The validator for DeleteSaleCommand</param>
        public DeleteSaleHandler(
            ISaleRepository saleRepository, ISaleItemRepository saleItemRepository)
        {
            _saleRepository = saleRepository;
            _saleItemRepository = saleItemRepository;
        }

        /// <summary>
        /// Handles the DeleteSaleCommand request
        /// </summary>
        /// <param name="request">The DeleteSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the delete operation</returns>
        public async Task<DeleteSaleResult> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteSaleValidator();
            var validationResult = await validator.ValidateAsync(request, options => options.ThrowOnFailures(), cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
            var products = await _saleItemRepository.GetBySaleIdAsync(request.Id, cancellationToken);
            if (products != null && products.Any())
            {
                foreach (var product in products)
                {
                    await _saleItemRepository.DeleteAsync(product.Id, cancellationToken);
                }
            }

            return new DeleteSaleResult { Success = true };
        }
    }
}
