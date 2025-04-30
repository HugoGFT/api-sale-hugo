using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// API response model for GetCart operation
/// </summary>
public class GetCartResponse
{
    /// <summary>
    /// The unique identifier of the Cart
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The Cart's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The Cart's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The Cart's phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;
}
