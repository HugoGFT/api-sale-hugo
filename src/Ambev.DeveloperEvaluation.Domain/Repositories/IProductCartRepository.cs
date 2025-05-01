using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductCartRepository
    {
        /// <summary>
        /// Creates a new ProductCart in the repository
        /// </summary>
        /// <param name="ProductCart">The ProductCart to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created ProductCart</returns>
        Task<ProductCart> CreateAsync(ProductCart ProductCart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a ProductCart by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the ProductCart</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ProductCart if found, null otherwise</returns>
        Task<ProductCart?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a ProductCart by their cart id
        /// </summary>
        /// <param name="idCart">The cart id to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ProductCart if found, null otherwise</returns>
        Task<List<ProductCart?>> GetByCartIdAsync(int idCart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a ProductCart from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the ProductCart to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the ProductCart was deleted, false if not found</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a ProductCart in the repository
        /// </summary>
        /// <param name="ProductCart">The ProductCart to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created ProductCart</returns>
        Task<ProductCart> UpdateAsync(ProductCart ProductCart, CancellationToken cancellationToken = default);
    }
}
