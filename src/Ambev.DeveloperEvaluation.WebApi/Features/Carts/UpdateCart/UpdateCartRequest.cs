using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Represents a request to create a new Cart in the system.
/// </summary>
public class UpdateCartRequest
{
    public int UserID { get; set; }
    public string Date { get; set; } = string.Empty;
    public List<ProductCartDto> Products { get; set; } = new List<ProductCartDto>();
}