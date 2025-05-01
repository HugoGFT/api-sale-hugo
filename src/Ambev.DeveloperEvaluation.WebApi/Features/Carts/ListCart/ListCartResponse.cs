using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCart;

/// <summary>
/// API response model for ListCart operation
/// </summary>
public class ListCartResponse
{
    public int TotalItems { get; set; }
    public IEnumerable<GetCartResponse> Data { get; set; } = new List<GetCartResponse>();
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
