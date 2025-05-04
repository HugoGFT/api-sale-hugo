using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of SaleItemRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public SaleItemRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new SaleItem in the database
        /// </summary>
        /// <param name="SaleItem">The SaleItem to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created SaleItem</returns>
        public async Task<SaleItem> CreateAsync(SaleItem SaleItem, CancellationToken cancellationToken = default)
        {
            await _context.SaleItems.AddAsync(SaleItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return SaleItem;
        }

        /// <summary>
        /// Retrieves a SaleItem by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the SaleItem</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The SaleItem if found, null otherwise</returns>
        public async Task<SaleItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SaleItems.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a SaleItem by their sale id
        /// </summary>
        /// <param name="saleId">The sale id to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ProductCart if found, null otherwise</returns>
        public async Task<List<SaleItem?>> GetBySaleIdAsync(int saleId, CancellationToken cancellationToken = default)
        {
            return await _context.SaleItems.AsNoTracking()
                .Where(u => u.SaleId == saleId)
                .Cast<SaleItem?>()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a SaleItem from the database
        /// </summary>
        /// <param name="id">The unique identifier of the SaleItem to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the SaleItem was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var SaleItem = await GetByIdAsync(id, cancellationToken);
            if (SaleItem == null)
                return false;

            _context.SaleItems.Remove(SaleItem);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Update a SaleItem in the database
        /// </summary>
        /// <param name="SaleItem">The SaleItem to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created SaleItem</returns>
        public async Task<SaleItem> UpdateAsync(SaleItem SaleItem, CancellationToken cancellationToken = default)
        {
            _context.SaleItems.Update(SaleItem);
            await _context.SaveChangesAsync(cancellationToken);
            return SaleItem;
        }
    }
}
