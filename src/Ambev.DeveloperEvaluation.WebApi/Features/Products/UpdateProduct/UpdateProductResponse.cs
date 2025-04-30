using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// API response model for UpdateProduct operation
/// </summary>
public class UpdateProductResponse
{
    /// <summary>
    /// The unique identifier of the Update Product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The Product's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The Product's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The Product's phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;
}
