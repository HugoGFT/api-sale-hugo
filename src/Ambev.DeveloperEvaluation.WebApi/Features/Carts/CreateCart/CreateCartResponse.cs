using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// API response model for CreateCart operation
/// </summary>
public class CreateCartResponse
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public string Date { get; set; } = string.Empty;
    public List<ProductCartDto> Products { get; set; } = new List<ProductCartDto>();
}
