namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Request model for deleting a Product
/// </summary>
public class DeleteProductRequest
{
    /// <summary>
    /// The identifier of the Product to delete
    /// </summary>
    public int Id { get; set; }
}
