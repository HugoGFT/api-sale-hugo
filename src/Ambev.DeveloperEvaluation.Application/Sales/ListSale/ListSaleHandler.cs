using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleHandler : IRequestHandler<ListSaleCommand, ListSaleResult>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly ISaleItemRepository _SaleItemRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ListSaleHandler
        /// </summary>
        /// <param name="SaleRepository">The Sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ListSaleHandler(
            ISaleRepository SaleRepository,
            IMapper mapper,
            ISaleItemRepository SaleItemRepository)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
            _SaleItemRepository = SaleItemRepository;
        }

        /// <summary>
        /// Handles the ListSaleCommand request
        /// </summary>
        /// <param name="request">The ListSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Sale details if found</returns>
        public async Task<ListSaleResult> Handle(ListSaleCommand request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ListSaleFilter>(request);
            var Sale = await _SaleRepository.GetByFilterAsync(filter, cancellationToken);

            if (Sale == null)
            {
                throw new Exception("Sale not found");
            }

            var result = _mapper.Map<ListSaleResult>(Sale);
            foreach (var SaleItem in result.Data)
            {
                SaleItem.SaleItems = _mapper.Map<List<SaleItemDto>>(await _SaleItemRepository.GetBySaleIdAsync(SaleItem.Id, cancellationToken));

            }
            return result;
        }
    }
}
