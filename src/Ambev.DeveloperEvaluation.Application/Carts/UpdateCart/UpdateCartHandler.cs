using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
    {
        private readonly ICartRepository _CartRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for UpdateCartCommand</param>
        public UpdateCartHandler(ICartRepository CartRepository, IMapper mapper)
        {
            _CartRepository = CartRepository;
            _mapper = mapper;
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

            var existingCart = await _CartRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingCart == null)
                throw new InvalidOperationException($"Cart with Id {command.Id} not exists");

            var Cart = _mapper.Map<Cart>(command);

            var updatedCart = await _CartRepository.UpdateAsync(Cart, cancellationToken);
            var result = _mapper.Map<UpdateCartResult>(updatedCart);
            return result;
        }
    }
}
