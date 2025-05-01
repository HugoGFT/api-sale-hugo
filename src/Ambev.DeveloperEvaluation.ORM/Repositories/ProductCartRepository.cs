using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class ProductCartRepository : IProductCartRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductCartRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public ProductCartRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new ProductCart in the database
        /// </summary>
        /// <param name="ProductCart">The ProductCart to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created ProductCart</returns>
        public async Task<ProductCart> CreateAsync(ProductCart ProductCart, CancellationToken cancellationToken = default)
        {
            await _context.ProductCarts.AddAsync(ProductCart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ProductCart;
        }

        /// <summary>
        /// Retrieves a ProductCart by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the ProductCart</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ProductCart if found, null otherwise</returns>
        public async Task<ProductCart?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ProductCarts.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a ProductCart by their cart id
        /// </summary>
        /// <param name="idCart">The cart id to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ProductCart if found, null otherwise</returns>
        public async Task<List<ProductCart?>> GetByCartIdAsync(int idCart, CancellationToken cancellationToken = default)
        {
            return await _context.ProductCarts
                .Where(u => u.IdCart == idCart)
                .Cast<ProductCart?>()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a ProductCart from the database
        /// </summary>
        /// <param name="id">The unique identifier of the ProductCart to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the ProductCart was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var ProductCart = await GetByIdAsync(id, cancellationToken);
            if (ProductCart == null)
                return false;

            _context.ProductCarts.Remove(ProductCart);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Update a ProductCart in the database
        /// </summary>
        /// <param name="ProductCart">The ProductCart to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created ProductCart</returns>
        public async Task<ProductCart> UpdateAsync(ProductCart ProductCart, CancellationToken cancellationToken = default)
        {
            _context.ProductCarts.Update(ProductCart);
            await _context.SaveChangesAsync(cancellationToken);
            return ProductCart;
        }
    }
}
