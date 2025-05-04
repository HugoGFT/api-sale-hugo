using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        /// <summary>
        /// Creates a new Sale in the repository
        /// </summary>
        /// <param name="Sale">The Sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Sale</returns>
        Task<Sale> CreateAsync(Sale Sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Sale by their identifier
        /// </summary>
        /// <param name="id">The identifier of the Sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Sale if found, null otherwise</returns>
        Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a Sale from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the Sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the Sale was deleted, false if not found</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a Sale in the repository
        /// </summary>
        /// <param name="Sale">The Sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Sale</returns>
        Task<Sale> UpdateAsync(Sale Sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Sale by a filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListSaleResultDto> GetByFilterAsync(ListSaleFilter filter, CancellationToken cancellationToken = default);
    }
}
