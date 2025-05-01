using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public record DeleteCartCommand : IRequest<DeleteCartResponse>
    {
        /// <summary>
        /// The unique identifier of the Cart to delete
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of DeleteCartCommand
        /// </summary>
        /// <param name="id">The ID of the Cart to delete</param>
        public DeleteCartCommand(int id)
        {
            Id = id;
        }
    }
}
