using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartHandler : IRequestHandler<ListCartCommand, ListCartResult>
    {
        private readonly ICartRepository _CartRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ListCartHandler
        /// </summary>
        /// <param name="CartRepository">The Cart repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ListCartHandler(
            ICartRepository CartRepository,
            IMapper mapper)
        {
            _CartRepository = CartRepository;
            _mapper = mapper;
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

            return _mapper.Map<ListCartResult>(Cart);
        }
    }
}
