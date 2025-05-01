using Ambev.DeveloperEvaluation.Domain.Dto.ProductDto;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Creates a new Product in the repository
        /// </summary>
        /// <param name="Product">The Product to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Product</returns>
        Task<Product> CreateAsync(Product Product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Product by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the Product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Product if found, null otherwise</returns>
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Product by their category
        /// </summary>
        /// <param name="category">The category to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Product if found, null otherwise</returns>
        Task<Product?> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a Product from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the Product to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the Product was deleted, false if not found</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a Product in the repository
        /// </summary>
        /// <param name="Product">The Product to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Product</returns>
        Task<Product> UpdateAsync(Product Product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Product by a filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListProductResultDto> GetByFilterAsync(ListProductFilter filter, CancellationToken cancellationToken = default);
    }
}
