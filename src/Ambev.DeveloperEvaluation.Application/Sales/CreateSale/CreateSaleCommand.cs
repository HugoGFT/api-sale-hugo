using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string? Date { get; set; }
        public int CustomerId { get; set; }
        public string? Branch { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItemDto> SaleItems { get; set; } = new List<SaleItemDto>();
    }
}
