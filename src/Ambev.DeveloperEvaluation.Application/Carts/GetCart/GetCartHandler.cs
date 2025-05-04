using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResult>
    {
        private readonly ICartRepository _CartRepository;
        private readonly IProductCartRepository _productCartRepository;
        private readonly IMapper _mapper;

        public GetCartHandler(
            ICartRepository CartRepository,
            IMapper mapper,
            IProductCartRepository productCartRepository)
        {
            _CartRepository = CartRepository;
            _mapper = mapper;
            _productCartRepository = productCartRepository;
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
            var result = _mapper.Map<GetCartResult>(Cart);
            result.Products = _mapper.Map<List<ProductCartDto>>(await _productCartRepository.GetByCartIdAsync(request.Id, cancellationToken));
            return result;
        }
    }
}
