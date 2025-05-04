using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly ISaleItemRepository _SaleItemRepository;
        private readonly IMapper _mapper;

        public GetSaleHandler(
            ISaleRepository SaleRepository,
            IMapper mapper,
            ISaleItemRepository SaleItemRepository)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
            _SaleItemRepository = SaleItemRepository;
        }

        /// <summary>
        /// Handles the GetSaleCommand request
        /// </summary>
        /// <param name="request">The GetSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Sale details if found</returns>
        public async Task<GetSaleResult> Handle(GetSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var Sale = await _SaleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (Sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
            var result = _mapper.Map<GetSaleResult>(Sale);
            result.SaleItems = _mapper.Map<List<SaleItemDto>>(await _SaleItemRepository.GetBySaleIdAsync(request.Id, cancellationToken));
            return result;
        }
    }
}
