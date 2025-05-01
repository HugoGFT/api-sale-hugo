using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ICartRepository
    {
        /// <summary>
        /// Creates a new Cart in the repository
        /// </summary>
        /// <param name="Cart">The Cart to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Cart</returns>
        Task<Cart> CreateAsync(Cart Cart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Cart by their identifier
        /// </summary>
        /// <param name="id">The identifier of the Cart</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Cart if found, null otherwise</returns>
        Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a Cart from the repository
        /// </summary>
        /// <param name="id">The unique identifier of the Cart to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the Cart was deleted, false if not found</returns>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a Cart in the repository
        /// </summary>
        /// <param name="Cart">The Cart to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Cart</returns>
        Task<Cart> UpdateAsync(Cart Cart, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Cart by a filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListCartResultDto> GetByFilterAsync(ListCartFilter filter, CancellationToken cancellationToken = default);
    }
}
