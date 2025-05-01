using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProduct;

/// <summary>
/// API response model for ListProduct operation
/// </summary>
public class ListProductResponse
{
    public int TotalItems { get; set; }
    public IEnumerable<GetProductResponse> Data { get; set; } = new List<GetProductResponse>();
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
