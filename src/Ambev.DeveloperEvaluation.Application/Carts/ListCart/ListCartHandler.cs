using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartHandler : IRequestHandler<ListCartCommand, ListCartResult>
    {
        private readonly ICartRepository _CartRepository;
        private readonly IProductCartRepository _productCartRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ListCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ListCartHandler(
            ICartRepository CartRepository,
            IMapper mapper,
            IProductCartRepository productCartRepository)
        {
            _CartRepository = CartRepository;
            _mapper = mapper;
            _productCartRepository = productCartRepository;
        }

        /// <summary>
        /// Handles the ListCartCommand request
        /// </summary>
        /// <param name="request">The ListCart command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Cart details if found</returns>
        public async Task<ListCartResult> Handle(ListCartCommand request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ListCartFilter>(request);
            var Cart = await _CartRepository.GetByFilterAsync(filter, cancellationToken);

            if (Cart == null)
            {
                throw new Exception("Cart not found");
            }

            var result = _mapper.Map<ListCartResult>(Cart);
            foreach (var productCart in result.Data)
            {
                var aux = await _productCartRepository.GetByCartIdAsync(productCart.Id, cancellationToken);
                productCart.Products = _mapper.Map<List<ProductCartDto>>(await _productCartRepository.GetByCartIdAsync(productCart.Id, cancellationToken));

            }
            return result;
        }
    }
}
