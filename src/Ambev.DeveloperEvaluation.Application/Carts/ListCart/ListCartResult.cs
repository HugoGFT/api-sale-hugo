using Ambev.DeveloperEvaluation.Application.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCart
{
    public class ListCartResult
    {
        public int TotalItems { get; set; }
        public IEnumerable<GetCartResult> Data { get; set; } = new List<GetCartResult>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
