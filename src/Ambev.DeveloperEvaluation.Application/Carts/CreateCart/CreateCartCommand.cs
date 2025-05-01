using Ambev.DeveloperEvaluation.Application.Carts.ListCart;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartCommand : IRequest<CreateCartResult>
    {
        public int UserID { get; set; }
        public string Date { get; set; } = string.Empty;
        public List<ProductCartDto> Products { get; set; } = new List<ProductCartDto>();
    }
}
