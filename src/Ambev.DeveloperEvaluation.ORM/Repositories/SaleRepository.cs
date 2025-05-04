using Ambev.DeveloperEvaluation.Domain.Dto.SaleDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of SaleRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Sale in the database
        /// </summary>
        /// <param name="Sale">The Sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Sale</returns>
        public async Task<Sale> CreateAsync(Sale Sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(Sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Sale;
        }

        /// <summary>
        /// Retrieves a Sale by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the Sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The Sale if found, null otherwise</returns>
        public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Deletes a Sale from the database
        /// </summary>
        /// <param name="id">The unique identifier of the Sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the Sale was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var Sale = await GetByIdAsync(id, cancellationToken);
            if (Sale == null)
                return false;

            _context.Sales.Remove(Sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Update a Sale in the database
        /// </summary>
        /// <param name="Sale">The Sale to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created Sale</returns>
        public async Task<Sale> UpdateAsync(Sale Sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(Sale);
            await _context.SaveChangesAsync(cancellationToken);
            return Sale;
        }

        /// <summary>
        /// Retrieves a list of Sales based on the provided filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ListSaleResultDto> GetByFilterAsync(ListSaleFilter filter, CancellationToken cancellationToken = default)
        {
            var totalItems = await _context.Sales.AsNoTracking().CountAsync(cancellationToken);
            var data = await _context.Sales.AsNoTracking()
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / filter.PageSize);
            return new ListSaleResultDto(totalItems, totalPages, filter.Page, data);
        }
    }
}
