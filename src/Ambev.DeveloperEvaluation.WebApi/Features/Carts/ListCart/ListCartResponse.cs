using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCart;

/// <summary>
/// API response model for ListCart operation
/// </summary>
public class ListCartResponse
{
    /// <summary>
    /// The unique identifier of the created Cart
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
