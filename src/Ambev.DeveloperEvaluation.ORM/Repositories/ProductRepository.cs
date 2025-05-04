using Ambev.DeveloperEvaluation.Domain.Dto.ProductDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Product in the database
        /// </summary>
        /// <param name="Product">The Product to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Product</returns>
        public async Task<Product> CreateAsync(Product Product, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(Product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Product;
        }

        /// <summary>
        /// Retrieves a Product by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the Product</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Product if found, null otherwise</returns>
        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a Product by their category
        /// </summary>
        /// <param name="cateory">The category to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Product if found, null otherwise</returns>
        public async Task<Product?> GetByCategoryAsync(string cateory, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Category == cateory, cancellationToken);
        }

        /// <summary>
        /// Deletes a Product from the database
        /// </summary>
        /// <param name="id">The unique identifier of the Product to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the Product was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var Product = await GetByIdAsync(id, cancellationToken);
            if (Product == null)
                return false;

            _context.Products.Remove(Product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Update a Product in the database
        /// </summary>
        /// <param name="Product">The Product to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Product</returns>
        public async Task<Product> UpdateAsync(Product Product, CancellationToken cancellationToken = default)
        {
            _context.Products.Update(Product);
            await _context.SaveChangesAsync(cancellationToken);
            return Product;
        }

        /// <summary>
        /// Retrieves a list of Products based on the provided filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListProductResultDto> GetByFilterAsync(ListProductFilter filter, CancellationToken cancellationToken = default)
        {
            var totalItems = await _context.Products.AsNoTracking().CountAsync(cancellationToken);
            var data = await _context.Products.AsNoTracking()
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize);
            return new ListProductResultDto(totalItems, totalPages, filter.Page, data);
        }
    }
}
