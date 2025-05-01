using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResult>
    {
        private readonly ICartRepository _CartRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for GetCartCommand</param>
        public GetCartHandler(
            ICartRepository CartRepository,
            IMapper mapper)
        {
            _CartRepository = CartRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetCartCommand request
        /// </summary>
        /// <param name="request">The GetCart command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Cart details if found</returns>
        public async Task<GetCartResult> Handle(GetCartCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetCartValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Cart = await _CartRepository.GetByIdAsync(request.Id, cancellationToken);
            if (Cart == null)
                throw new KeyNotFoundException($"Cart with ID {request.Id} not found");

            return _mapper.Map<GetCartResult>(Cart);
        }
    }
}
