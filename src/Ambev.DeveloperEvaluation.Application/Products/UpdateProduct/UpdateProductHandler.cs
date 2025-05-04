using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateProductHandler
        /// </summary>
        /// <param name="ProductRepository">The Product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for UpdateProductCommand</param>
        public UpdateProductHandler(IProductRepository ProductRepository, IMapper mapper)
        {
            _ProductRepository = ProductRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the UpdateProductCommand request
        /// </summary>
        /// <param name="command">The UpdateProduct command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Updated Product details</returns>
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Product = _mapper.Map<Product>(command);

            var updatedProduct = await _ProductRepository.UpdateAsync(Product, cancellationToken);
            var result = _mapper.Map<UpdateProductResult>(updatedProduct);
            return result;
        }
    }
}
