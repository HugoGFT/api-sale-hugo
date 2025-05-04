using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _SaleRepository;
        private readonly ISaleItemRepository _SaleItemRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateSaleHandler
        /// </summary>
        /// <param name="SaleRepository">The Sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for UpdateSaleCommand</param>
        public UpdateSaleHandler(ISaleRepository SaleRepository, IMapper mapper, ISaleItemRepository SaleItemRepository)
        {
            _SaleRepository = SaleRepository;
            _mapper = mapper;
            _SaleItemRepository = SaleItemRepository;
        }

        /// <summary>
        /// Handles the UpdateSaleCommand request
        /// </summary>
        /// <param name="command">The UpdateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Updated Sale details</returns>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _SaleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingSale == null)
                throw new InvalidOperationException($"Sale with Id {command.Id} not exists");

            var sale = _mapper.Map<Sale>(command);
            command.SaleItems.ForEach(x => x.ApplyDiscount());
            sale.TotalAmount = command.SaleItems.Where(x => x.IsCancelled == false).Sum(x => x.Total);
            sale.TotalDiscount = command.SaleItems.Where(x => x.IsCancelled == false).Sum(x => x.Discount);
            sale.TotalWithDiscount = sale.TotalAmount - sale.TotalDiscount;
            var updatedSale = await _SaleRepository.UpdateAsync(sale, cancellationToken);
            var saleItems = _mapper.Map<List<SaleItem>>(command.SaleItems);
            var existingSaleItems = await _SaleItemRepository.GetBySaleIdAsync(updatedSale.Id, cancellationToken);

            foreach (var saleItem in saleItems)
            {
                var existingSaleItem = existingSaleItems.FirstOrDefault(p => p?.ProductId == saleItem.ProductId);
                if (existingSaleItem != null)
                {
                    saleItem.Id = existingSaleItem.Id;
                    saleItem.SaleId = updatedSale.Id;
                    await _SaleItemRepository.UpdateAsync(saleItem, cancellationToken);
                }
                else
                {
                    saleItem.SaleId = updatedSale.Id;
                    await _SaleItemRepository.CreateAsync(saleItem, cancellationToken);
                }
            }
            var deleteSaleItems = existingSaleItems.Where(o => !saleItems.Any(p => p.ProductId == o?.ProductId)).ToList();
            foreach (var saleItem in deleteSaleItems)
            {
                if (saleItem == null)
                    continue;
                await _SaleItemRepository.DeleteAsync(saleItem.Id, cancellationToken);
            }
            var result = _mapper.Map<UpdateSaleResult>(updatedSale);
            result.SaleItems = _mapper.Map<List<SaleItemDto>>(await _SaleItemRepository.GetBySaleIdAsync(result.Id, cancellationToken));
            return result;
        }
    }
}
