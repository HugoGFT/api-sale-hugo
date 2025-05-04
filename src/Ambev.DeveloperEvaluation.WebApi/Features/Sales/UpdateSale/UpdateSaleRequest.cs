using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequest
    {
        public string Date { get; set; }
        public int CustomerId { get; set; }
        public string Branch { get; set; }
        public bool IsCancelled { get; set; }
        public List<CreateSaleItemDto> SaleItems { get; set; } = new List<CreateSaleItemDto>();

    }
}
