using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale
{
    public class ListSaleResponse
    {
        public int TotalItems { get; set; }
        public IEnumerable<GetSaleResponse> Data { get; set; } = new List<GetSaleResponse>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
