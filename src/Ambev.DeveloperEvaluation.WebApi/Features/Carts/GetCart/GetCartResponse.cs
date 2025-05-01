using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// API response model for GetCart operation
/// </summary>
public class GetCartResponse
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public string Date { get; set; } = string.Empty;
    public List<ProductCartDto> Products { get; set; } = new List<ProductCartDto>();
}
