using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartCommand : IRequest<UpdateCartResult>
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Date { get; set; } = string.Empty;
        public List<ProductCartDto> Products { get; set; } = new List<ProductCartDto>();
    }
}
