using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleResult
    {
        public int TotalItems { get; set; }
        public IEnumerable<GetSaleResult> Data { get; set; } = new List<GetSaleResult>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
