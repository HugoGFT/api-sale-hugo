using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Dto.ProductDto
{
    public class ListProductResultDto
    {
        public ListProductResultDto(int totalItems, int totalPages, int currentPage, List<Product> Products)
        {
            TotalItems = totalItems;
            Data = Products;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
        public int TotalItems { get; set; }
        public List<Product> Data { get; set; } = new List<Product>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
