using Ambev.DeveloperEvaluation.Domain.Dto.CartDto;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartResult
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Date { get; set; } = string.Empty;
        public List<ProductCartDto> Products { get; set; } = new List<ProductCartDto>();
    }
}
