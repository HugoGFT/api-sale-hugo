using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly ISaleItemRepository _SaleItemRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="SaleRepository">The Sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for CreateSaleCommand</param>
        public CreateSaleHandler(ISaleRepository SaleRepository, IMapper mapper, ISaleItemRepository SaleItemRepository)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
            _SaleItemRepository = SaleItemRepository;
        }

        /// <summary>
        /// Handles the CreateSaleCommand request
        /// </summary>
        /// <param name="command">The CreateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Sale details</returns>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = _mapper.Map<Sale>(command);
            command.SaleItems.ForEach(x => x.ApplyDiscount());
            sale.TotalAmount = command.SaleItems.Where(x => x.IsCancelled == false).Sum(x => x.Total);
            sale.TotalDiscount = command.SaleItems.Where(x => x.IsCancelled == false).Sum(x => x.Discount);
            sale.TotalWithDiscount = sale.TotalAmount - sale.TotalDiscount;


            var createdSale = await _SaleRepository.CreateAsync(sale, cancellationToken);
            var saleItems = _mapper.Map<List<SaleItem>>(command.SaleItems);
            foreach (var saleItem in saleItems)
            {
                saleItem.SaleId = createdSale.Id;
                await _SaleItemRepository.CreateAsync(saleItem, cancellationToken);
            }
            var result = _mapper.Map<CreateSaleResult>(createdSale);
            result.SaleItems = _mapper.Map<List<SaleItemDto>>(await _SaleItemRepository.GetBySaleIdAsync(result.Id, cancellationToken));
            return result;
        }
    }
}
