using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public record DeleteSaleCommand : IRequest<DeleteSaleResult>
    {
        /// <summary>
        /// The unique identifier of the Sale to delete
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of DeleteSaleCommand
        /// </summary>
        /// <param name="id">The ID of the Sale to delete</param>
        public DeleteSaleCommand(int id)
        {
            Id = id;
        }
    }
}
