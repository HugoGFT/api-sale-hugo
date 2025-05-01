using Ambev.DeveloperEvaluation.Application.Carts.ListCart;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartCommand : IRequest<GetCartResult>
    {
        public int Id { get; set; }
    }
}
