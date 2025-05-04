using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleItemRepository
    {
        /// <summary>
        /// Creates a new SaleItem in the repository
        /// </summary>
        /// <param name="SaleItem">The SaleItem to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created SaleItem</returns>
        Task<SaleItem> CreateAsync(SaleItem SaleItem, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a SaleItem by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the SaleItem</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The SaleItem if found, null otherwise</returns>
        Task<SaleItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a SaleItem by their cart id
        /// </summary>
        /// <param name="id">The sale id to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The SaleItem if found, null otherwise</returns>
        Task<List<SaleItem?>> GetBySaleIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a SaleItem from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the SaleItem to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the SaleItem was deleted, false if not found</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a SaleItem in the repository
        /// </summary>
        /// <param name="SaleItem">The SaleItem to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created SaleItem</returns>
        Task<SaleItem> UpdateAsync(SaleItem SaleItem, CancellationToken cancellationToken = default);
    }
}
