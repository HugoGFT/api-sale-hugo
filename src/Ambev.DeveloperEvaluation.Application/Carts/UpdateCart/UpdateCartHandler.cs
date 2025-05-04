using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductCartRepository _productCartRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for UpdateCartCommand</param>
        public UpdateCartHandler(ICartRepository cartRepository, IMapper mapper, IProductCartRepository productCartRepository)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _productCartRepository = productCartRepository;
        }

        /// <summary>
        /// Handles the UpdateCartCommand request
        /// </summary>
        /// <param name="command">The UpdateCart command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Updated Cart details</returns>
        public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateCartCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingCart == null)
                throw new InvalidOperationException($"Cart with Id {command.Id} not exists");

            var Cart = _mapper.Map<Cart>(command);

            var updatedCart = await _cartRepository.UpdateAsync(Cart, cancellationToken);
            var productCarts = _mapper.Map<List<ProductCart>>(command.Products);
            var existingProductCarts = await _productCartRepository.GetByCartIdAsync(updatedCart.Id, cancellationToken);

            foreach (var productCart in productCarts)
            {
                productCart.IdUser = command.UserID;
                productCart.IdCart = updatedCart.Id;
                var existingProductCart = existingProductCarts?.FirstOrDefault( x => x?.IdProduct == productCart.IdProduct);
                if (existingProductCart != null)
                {
                    productCart.Id = existingProductCart.Id;
                    await _productCartRepository.UpdateAsync(productCart, cancellationToken);
                }
                else
                {
                    await _productCartRepository.CreateAsync(productCart, cancellationToken);
                }
            }
            var deleteProductCarts = existingProductCarts?.Where(o => !productCarts.Any(p => p.IdProduct == o?.IdProduct)).ToList();
            if (deleteProductCarts?.Any() == true)
            {
                foreach (var productCart in deleteProductCarts)
                {
                    if (productCart != null)
                        await _productCartRepository.DeleteAsync(productCart.Id, cancellationToken);
                }
            }
            var result = _mapper.Map<UpdateCartResult>(updatedCart);
            result.Products = _mapper.Map<List<ProductCartDto>>(await _productCartRepository.GetByCartIdAsync(result.Id, cancellationToken));
            return result;
        }
    }
}
