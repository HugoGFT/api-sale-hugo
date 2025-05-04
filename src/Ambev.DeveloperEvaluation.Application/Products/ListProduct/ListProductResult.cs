using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductResult
    {
        public int TotalItems { get; set; }
        public IEnumerable<GetProductResult> Data { get; set; } = new List<GetProductResult>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
