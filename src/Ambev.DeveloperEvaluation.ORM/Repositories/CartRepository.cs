using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of CartRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public CartRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Cart in the database
        /// </summary>
        /// <param name="Cart">The Cart to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Cart</returns>
        public async Task<Cart> CreateAsync(Cart Cart, CancellationToken cancellationToken = default)
        {
            await _context.Carts.AddAsync(Cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Cart;
        }

        /// <summary>
        /// Retrieves a Cart by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the Cart</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Cart if found, null otherwise</returns>
        public async Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Carts.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Deletes a Cart from the database
        /// </summary>
        /// <param name="id">The unique identifier of the Cart to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the Cart was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var Cart = await GetByIdAsync(id, cancellationToken);
            if (Cart == null)
                return false;

            _context.Carts.Remove(Cart);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Update a Cart in the database
        /// </summary>
        /// <param name="Cart">The Cart to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Cart</returns>
        public async Task<Cart> UpdateAsync(Cart Cart, CancellationToken cancellationToken = default)
        {
            _context.Carts.Update(Cart);
            await _context.SaveChangesAsync(cancellationToken);
            return Cart;
        }

        /// <summary>
        /// Retrieves a list of Carts based on the provided filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListCartResultDto> GetByFilterAsync(ListCartFilter filter, CancellationToken cancellationToken = default)
        {
            var totalItems = await _context.Carts.CountAsync(cancellationToken);
            var data = await _context.Carts
                .Skip(filter.Page * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize);
            return new ListCartResultDto(totalItems, totalPages, filter.Page, data);
        }
    }
}
