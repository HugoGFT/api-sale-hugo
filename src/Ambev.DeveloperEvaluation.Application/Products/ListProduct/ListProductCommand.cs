using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
{
    public class ListProductCommand : IRequest<ListProductResult>
    {
        public ListProductCommand(int page, int size, string order)
        {
            Page = page;
            PageSize = size;
            Order = order;
        }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Order { get; set; } = "asc";
    }
}
