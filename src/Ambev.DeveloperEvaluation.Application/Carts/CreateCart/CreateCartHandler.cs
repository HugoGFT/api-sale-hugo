using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductCartRepository _productCartRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CreateCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for CreateCartCommand</param>
        public CreateCartHandler(ICartRepository cartRepository, IMapper mapper, IProductCartRepository productCartRepository)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _productCartRepository = productCartRepository;
        }

        /// <summary>
        /// Handles the CreateCartCommand request
        /// </summary>
        /// <param name="command">The CreateCart command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Cart details</returns>
        public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateCartCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Cart = _mapper.Map<Cart>(command);

            var createdCart = await _cartRepository.CreateAsync(Cart, cancellationToken);
            var productCarts = _mapper.Map<List<ProductCart>>(command.Products);
            foreach (var productCart in productCarts)
            {
                productCart.IdCart = createdCart.Id;
                await _productCartRepository.CreateAsync(productCart, cancellationToken);
            }
            var result = _mapper.Map<CreateCartResult>(createdCart);
            return result;
        }
    }

}
