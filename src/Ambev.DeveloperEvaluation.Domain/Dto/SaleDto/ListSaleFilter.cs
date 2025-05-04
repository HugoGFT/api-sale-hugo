namespace Ambev.DeveloperEvaluation.Domain.Dto.SaleDto
{
    public class ListSaleFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Order { get; set; }
    }
}
