using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleResult
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int CustomerId { get; set; }
        public string Branch { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalWithDiscount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItemDto> SaleItems { get; set; } = new List<SaleItemDto>();
    }
}
