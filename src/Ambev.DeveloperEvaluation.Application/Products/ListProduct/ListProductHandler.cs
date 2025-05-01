using Ambev.DeveloperEvaluation.Domain.Dto.ProductDto;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductHandler : IRequestHandler<ListProductCommand, ListProductResult>
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ListProductHandler
        /// </summary>
        /// <param name="ProductRepository">The Product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ListProductHandler(
            IProductRepository ProductRepository,
            IMapper mapper)
        {
            _ProductRepository = ProductRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the ListProductCommand request
        /// </summary>
        /// <param name="request">The ListProductCommand</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Product details if found</returns>
        public async Task<ListProductResult> Handle(ListProductCommand request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ListProductFilter>(request);
            var Product = await _ProductRepository.GetByFilterAsync(filter, cancellationToken);

            return _mapper.Map<ListProductResult>(Product);
        }
    }
}
